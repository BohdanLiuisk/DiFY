import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, concatLatestFrom, createEffect, ofType } from '@ngrx/effects';
import { CallHistoryService } from '../services/call-history.service';
import { CallHistoryFacade } from './call-history.facade';
import { callHistoryActions } from './call-history.actions';
import { catchError, concatMap, map, of, tap } from 'rxjs';

@Injectable()
export class CallHistoryEffects {
  constructor(
    private router: Router,
    private actions$: Actions,
    private callService: CallHistoryService,
    private facade: CallHistoryFacade
  ) { }

  public readonly createNewCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(callHistoryActions.createNewCall),
      concatMap(({ name, participantIds }) => this.callService.createNew({ name, participantIds }).pipe(
        map(({ callId }) =>
        callHistoryActions.joinCall({ callId })
        )
      ))
    )
  });

  public readonly joinCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(callHistoryActions.joinCall),
      tap(({ callId }) => {
        this.router.navigate([`home/call/${callId}`]);
      })
    )
  }, { dispatch: false });

  public readonly setHistoryPage = createEffect(() =>
    this.actions$.pipe(
      ofType(callHistoryActions.setHistoryPage, callHistoryActions.setPerPage),
      map(() => callHistoryActions.loadCalls()),
    )
  );

  public readonly loadCalls = createEffect(() =>
    this.actions$.pipe(
      ofType(callHistoryActions.loadCalls),
      concatLatestFrom(() => this.facade.historyConfig$),
      concatMap(([_, config]) =>
        this.callService.getAll(config.page, config.perPage).pipe(
          map(({items, totalCount}) =>
          callHistoryActions.loadCallsSuccess({ calls: items, totalCount })
          ),
          catchError((error) => of(callHistoryActions.loadCallsFailure({ error }))),
        )
      )
    )
  );
}
