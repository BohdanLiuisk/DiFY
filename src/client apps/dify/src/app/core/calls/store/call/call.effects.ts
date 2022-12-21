import { Injectable } from "@angular/core";
import { Actions, concatLatestFrom, createEffect, ofType } from "@ngrx/effects";
import { CallFacade } from "@core/calls/store/call/call.facade";
import { CallService } from "@core/calls/store/call/call.service";
import { callActions } from '@core/calls/store/call/call.actions';
import { catchError, map, of, switchMap, tap, merge, combineLatest, filter } from "rxjs";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { createSignalRHub, startSignalRHub, ofHub, signalrHubUnstarted, signalrConnected, mergeMapHubToAction, findHub, hubNotFound, signalrError } from "ngrx-signalr-core";
import { callHub } from "./call.hub";
import { IHttpConnectionOptions } from "@microsoft/signalr";
import { JwtStorageService } from "@core/auth/jwt-storage.service";
import { AuthFacade } from "@core/auth/store/auth.facade";
import { AuthService } from "@core/auth/auth.service";

@Injectable()
export class CallEffects {
  constructor(
    private actions$: Actions,
    private callService: CallService,
    private facade: CallFacade,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private jwtStorageService: JwtStorageService
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
      switchMap(() => this.authService.getJwtToken()),
      filter(({ access_token }) => Boolean(access_token)),
      map(({ access_token }) => {
        const options: IHttpConnectionOptions = {
          accessTokenFactory: () => access_token
        };
        return createSignalRHub({ ...callHub, options, automaticReconnect: true });
      })
    );
  });

  public readonly setCurrentMediaStream = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.setCurrentMediaStream),
      switchMap(({ stream }) => stream),
      map(stream => callActions.getCurrentMediaStreamSuccess({ stream }))
    );
  });

  public readonly initCallHub$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(signalrHubUnstarted),
      ofHub(callHub),
      map((hub) => startSignalRHub(hub))
    );
  });

  public readonly listenCallEvents$ = createEffect(() =>
    this.actions$.pipe(
      ofType(signalrConnected),
      ofHub(callHub),
      mergeMapHubToAction(({ hub }) => {
        const setLoaded$ = of(callActions.setLoaded());
        const testReceiveMessage$ = hub
          .on("ReceiveMessage")
          .pipe(map((message) => {
            return callActions.testReceiveMessage({ message })
          }));
        return merge(setLoaded$, testReceiveMessage$);
      })
    )
  );

  public readonly sendTestMessage$ = createEffect(() =>
      this.actions$.pipe(
        ofType(callActions.testSendMessage),
        map(({ message }) => {
          const hub = findHub(callHub);
          if (!hub) {
            return of(hubNotFound(callHub));
          }
          return hub.send("SendMessage", message, 'bbb');
        })
      ),
    { dispatch: false }
  );

  public readonly signalrError$ = createEffect(() =>
      this.actions$.pipe(
        ofType(signalrError),
        tap(({ error }) => {
          this.snackBar.open(error, 'Ok', {
            horizontalPosition: 'right',
            verticalPosition: 'top',
            panelClass: ['error-snackbar'],
            duration: 2000
          });
        })
      ),
    { dispatch: false }
  );
}
