import { Injectable } from "@angular/core";
import { Actions, concatLatestFrom, createEffect, ofType } from "@ngrx/effects";
import { CallFacade } from "@core/calls/store/call/call.facade";
import { CallService } from "@core/calls/store/call/call.service";
import { callActions } from '@core/calls/store/call/call.actions';
import { catchError, map, of, switchMap, tap, merge, filter } from "rxjs";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { callHub } from "./call.hub";
import { IHttpConnectionOptions } from "@microsoft/signalr";
import { AuthService } from "@core/auth/auth.service";
import { CallSignalrEvents } from "@core/calls/store/call-signalr.events";
import { CallConnectionData } from "@core/calls/store/call/call.models";
import { GUID } from "@shared/custom-types";
import { createHub, findHub, removeHub } from "@core/signalr/signalr";

@Injectable()
export class CallEffects {
  constructor(
    private actions$: Actions,
    private callService: CallService,
    private facade: CallFacade,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private signarEvents: CallSignalrEvents
  ) { }

  public readonly loadCall = createEffect(() =>
    this.actions$.pipe(
      ofType(callActions.loadCall),
      concatLatestFrom(() => this.facade.callId$),
      switchMap(([_, callId]) =>
        this.callService.getById(callId).pipe(
          map(({ call, participants }) =>
            callActions.loadCallSuccess({ call, participants })
          ),
          catchError((error) => of(callActions.loadCallFailure({ error })))
        )
      )
    )
  );

  public readonly loadCallFailure = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.loadCallFailure),
      tap(({ error }) => {
        this.snackBar.open(error.message, 'Ok', {
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: ['error-snackbar'],
          duration: 2000
        });
        this.router.navigate([`home/calls`]);
      })
    );
  }, { dispatch: false });

  public readonly loadCallSucess = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.loadCallSuccess),
      switchMap(() => {
        const hub = findHub(callHub);
        if(hub) {
          return hub.status$;
        } else {
          return of('unstarted');
        }
      }),
      filter(status => status === 'unstarted'),
      switchMap(() => this.authService.getJwtToken()),
      filter(({ access_token }) => Boolean(access_token)),
      switchMap(({ access_token }) => {
        const options: IHttpConnectionOptions = {
          accessTokenFactory: () => access_token
        };
        const hub = createHub(callHub.hubName, callHub.hubUrl, options, true);
        return hub.start().pipe(map(() => callActions.callHubStarted()));
      }),
      catchError((error) => of(callActions.callHubError({ error })))
    )
  });

  public readonly listenCallHubEvents$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.callHubStarted),
      switchMap(() => {
        const setLoaded$ = of(callActions.setLoaded());
        const hub = findHub(callHub);
        const participantJoinedCall$ = hub
          .on<CallConnectionData>("OnParticipantJoined")
          .pipe(
            tap((connection) => this.signarEvents.callParticipantConnected.next(connection)),
            map((connection) => callActions.addParticipant(connection.participant))
          );
        const participantLeftCall$ = hub
          .on<{ participantId: GUID }>("OnParticipantLeft")
          .pipe(
            tap(({ participantId }) => this.signarEvents.participantLeft.next({ participantId })),
            map(({ participantId }) => callActions.removeParticipant({ participantId }))
          );
        const hubStatus$ = hub.status$.pipe(
          filter(status => status === 'disconnected' || status === 'reconnecting'),
          switchMap((status) => {
            return of(callActions.callHubError( { error: { message: `Call hub status: ${ status }` }}));
          }));
        return merge(setLoaded$, participantJoinedCall$, participantLeftCall$, hubStatus$);
      })
    )
  });

  public readonly stopCallHub$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.stopCallHub),
      switchMap(() => {
        const hub = findHub(callHub);
        return hub.stop().pipe(switchMap(() => {
          return of(callActions.callHubStopped({ result: `Call hub stopped successfully`}));
        }));
      }),
      catchError((error) => of(callActions.callHubError({ error }))))
    }
  );

  public readonly callHubStopped$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.callHubStopped),
      tap(({ result }) => {
        removeHub(findHub(callHub));
        this.snackBar.open(result.toString(), 'Ok', {
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: ['error-snackbar'],
          duration: 2000
        });
      }))
    }, { dispatch: false }
  );

  public readonly callHubError$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.callHubError),
      tap(({ error }) => {
        const message = error && error.message;
        this.snackBar.open(message, 'Ok', {
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: ['error-snackbar'],
          duration: 2000
        });
      }))
    }, { dispatch: false }
  );

  public readonly invokeParticipantJoined$ = createEffect(() =>
    this.actions$.pipe(
      ofType(callActions.invokeParticipantJoined),
      switchMap((connection) => {
        const hub = findHub(callHub);
        if(!hub) {
          return of(() => callActions.callHubError({ error: { message: `Call hub not found` }}));
        }
        return hub.send("OnParticipantJoined", connection);
      })
    ), { dispatch: false }
  );
}
