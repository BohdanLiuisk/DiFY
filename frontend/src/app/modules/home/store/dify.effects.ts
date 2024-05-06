import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, filter, map, merge, of, switchMap, tap } from 'rxjs';
import { createHub, findHub } from '@core/signalr/signalr';
import { AuthService } from '@core/auth/services/auth.service';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { difyActions } from './dify.actions';
import { IncomingCallEvent } from '../models/dify.models';
import { DifySignalrEventsService } from '../services/dify-signalr.events';
import { Router } from '@angular/router';
import { HomeService } from '../services/home.service';
import { MessageService } from 'primeng/api';

const difyHub = environment.hubs.difyHub;

@Injectable()
export class DifyEffects {
  constructor(
    private actions$: Actions,
    private authService: AuthService,
    private router: Router,
    private difySignalrEvents: DifySignalrEventsService,
    private readonly homeService: HomeService,
    private messageService: MessageService) { }

  public readonly connectDifyHub = createEffect(() => {
    return this.actions$.pipe(
      ofType(difyActions.successfullyAuthenticated),
      switchMap(() => {
        const hub = findHub(difyHub);
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
        const hub = createHub(difyHub.hubName, difyHub.hubUrl, options, true);
        return hub.start();
      }),
      switchMap(() => {
        return of(difyActions.difyHubStarted());
      }),
      catchError((error) => of(difyActions.difyHubStatusChanged({ status: error })))
    )
  });

  public readonly difyHubStarted$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(difyActions.difyHubStarted),
      switchMap(() => {
        const hub = findHub(difyHub);
        const incomingCall$ = hub
          .on<IncomingCallEvent>("OnIncomingCall")
          .pipe(
            tap((incomingCall) => this.difySignalrEvents.incomingCallNotification.next(incomingCall)),
            map((incomingCall) => {
              return difyActions.incomingCallNotification(incomingCall);
            })
          );
        return merge(
          incomingCall$
        );
      })
    )
  });

  public readonly joinIncomingCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(difyActions.joinIncomingCall),
      switchMap(({ callId }) => 
        this.homeService.getCanJoinCall(callId).pipe(
          map(response => ({ callId, response }))
        )),
      tap(({ callId, response }) => {
        if(response.success) {
          this.router.navigate([`home/call/${ callId }`]);
        } else {
          this.messageService.add({ 
            severity: 'error', 
            summary: 'Error', 
            detail: response.errorMessage
          });
        }
      })
    )
  }, { dispatch: false });

  public readonly declineIncomingCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(difyActions.declineIncomingCall),
      switchMap(({ callId }) => {
        const hub = findHub(difyHub);
        return hub.send("OnDeclineIncomingCall", { callId });
      })
    )
  }, { dispatch: false });
}
