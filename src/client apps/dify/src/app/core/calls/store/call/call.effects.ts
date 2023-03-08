import { Inject, Injectable } from "@angular/core";
import { Actions, concatLatestFrom, createEffect, ofType } from "@ngrx/effects";
import { CallFacade } from "@core/calls/store/call/call.facade";
import { callActions } from '@core/calls/store/call/call.actions';
import { catchError, map, of, switchMap, tap, merge, filter, from, exhaustMap } from "rxjs";
import { IHttpConnectionOptions } from "@microsoft/signalr";
import { AuthService } from "@core/auth/auth.service";
import { CallSignalrEvents } from "@core/calls/store/call-signalr.events";
import { Call, JoinData, Participant } from "@core/calls/store/call/call.models";
import { GUID } from "@shared/custom-types";
import { createHub, findHub, removeHub } from "@core/signalr/signalr";
import { TuiAlertService, TuiNotification } from "@taiga-ui/core";
import { NavigationService } from "@core/services/navigation.service";
import { environment } from "@env/environment";

const callHub = environment.hubs.callHub;

@Injectable()
export class CallEffects {
  constructor(
    private actions$: Actions,
    private facade: CallFacade,
    private navigationService: NavigationService,
    private authService: AuthService,
    private signarEvents: CallSignalrEvents,
    @Inject(TuiAlertService)
    private readonly alertService: TuiAlertService
  ) { }

  public readonly startCallHub = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.startCallHub),
      switchMap(() => {
        const hub = findHub(callHub);
        if(hub) {
          return hub.status$;
        } else {
          return of('unstarted');
        }
      }),
      filter(status => status === 'unstarted' || status === 'disconnected'),
      switchMap(() => this.authService.getJwtToken()),
      filter(({ access_token }) => Boolean(access_token)),
      switchMap(({ access_token }) => {
        const options: IHttpConnectionOptions = {
          accessTokenFactory: () => access_token
        };
        const hub = createHub(callHub.hubName, callHub.hubUrl, options, true);
        return hub.start();
      }),
      map(() => {
        return callActions.callHubStarted();
      }),
      catchError((error) => of(callActions.callHubInfo({ info: error })))
    );
  });

  public readonly joinCall$ = createEffect(() =>
    this.actions$.pipe(
      ofType(callActions.joinCall),
      concatLatestFrom(() => this.facade.callId$),
      switchMap(([ { peerId }, callId ]) => from(navigator.mediaDevices.getUserMedia({ video: true, audio: false })).pipe(
        switchMap((stream) => {
          const hub = findHub(callHub);
          return hub.status$.pipe(
            filter(status => status === 'connected'),
            switchMap(() => {
              const joinData: JoinData = { peerId, streamId: stream.id, callId }
              return hub.send<{ call: Call; participants: Participant[] }>("OnJoinCall", joinData).pipe(
                switchMap((call) => {
                  return of(callActions.joinCallSuccess({ ...call, stream }));
                }),
                catchError((error) => {
                  return of(callActions.joinCallFailure({ error }));
                })
              );
            })
          );
        })
      ))
    )
  );

  public readonly enableVideoStream$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.enableVideoStream),
      exhaustMap(() => {
        return from(navigator.mediaDevices.getUserMedia({
          video: true,
          audio: false
        })).pipe(exhaustMap((mediaStream) => {
          const videoTrack = mediaStream.getVideoTracks()[0];
          this.signarEvents.updateVideoTrack.next({ videoTrack })
          return of(callActions.setNewVideoStream({ videoTrack }));
        }));
      })
  )});

  public readonly joinCallFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(callActions.joinCallFailure),
      tap(({ error }) => {
        this.alertService.open(error.toString(), { status: TuiNotification.Error }).subscribe();
        this.navigationService.back();
      })
    ), { dispatch: false });

  public readonly invokeParticipantJoined$ = createEffect(() =>
    this.actions$.pipe(
      ofType(callActions.invokeParticipantJoined),
      concatLatestFrom(() => this.facade.callId$),
      switchMap(([_, callId]) => {
        const hub = findHub(callHub);
        return hub.send("OnParticipantJoined", callId).pipe(
          switchMap(() => of(callActions.setLoaded()))
        );
      })
    )
  );

  public readonly participantLeftCall$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.removeParticipant),
      concatLatestFrom(({ participantId }) => this.facade.selectParticipantById(participantId)),
      tap(([_, participant]) => {
        const message = `${ participant.name } left call`;
        this.alertService.open(message, { status: TuiNotification.Info }).subscribe();
      }))
    }, { dispatch: false }
  );

  public readonly participantJoinedCall$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.addParticipant),
      tap(({ name }) => {
        const message = `${ name } joined call call`;
        this.alertService.open(message, { status: TuiNotification.Success }).subscribe();
      }))
    }, { dispatch: false }
  );

  public readonly listenCallHubEvents$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.joinCallSuccess),
      switchMap(({ stream }) => {
        const hub = findHub(callHub);
        const addParticipantCard$ = of(callActions.addParticipantCard({ stream }));
        const invokeParticipantJoined$ = of(callActions.invokeParticipantJoined());
        const participantJoinedCall$ = hub
          .on<Participant>("OnParticipantJoined")
          .pipe(
            tap((participant) => this.signarEvents.callParticipantConnected.next(participant)),
            map((participant) => callActions.addParticipant(participant))
          );
        const participantLeftCall$ = hub
          .on<{ participantId: GUID }>("OnParticipantLeft")
          .pipe(
            tap(({ participantId }) => this.signarEvents.participantLeft.next({ participantId })),
            map(({ participantId }) => callActions.removeParticipant({ participantId }))
          );
        const hubStatus$ = hub.status$.pipe(
          switchMap((status) => {
            return of(callActions.callHubInfo( { info: { message: `Call hub status: ${ status }` }}));
          }));
        return merge(
          addParticipantCard$,
          invokeParticipantJoined$,
          participantJoinedCall$,
          participantLeftCall$,
          hubStatus$
        );
      })
    )
  });

  public readonly stopCallHub$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.stopCallHub),
      switchMap(() => {
        const hub = findHub(callHub);
        return hub.stop().pipe(switchMap(() => {
          return of(callActions.callHubStopped());
        }));
      }),
      catchError((error) => of(callActions.callHubInfo({ info: error }))))
    }
  );

  public readonly callHubStopped$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.callHubStopped),
      tap(() => {
        removeHub(findHub(callHub));
      }))
    }, { dispatch: false }
  );

  public readonly callHubInfo$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(callActions.callHubInfo),
      tap(({ info }) => {
        const message = info && info.message;
        this.alertService.open(message, { status: TuiNotification.Info }).subscribe();
      }))
    }, { dispatch: false }
  );
}
