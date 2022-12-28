import { Injectable } from '@angular/core';
import { CallListService } from '@core/calls/store/call-list/call-list.service';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { Actions, concatLatestFrom, createEffect, ofType } from '@ngrx/effects';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import { catchError, concatMap, map, of, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class CallListEffects {
  constructor(
    private router: Router,
    private actions$: Actions,
    private callService: CallListService,
    private facade: CallListFacade
  ) { }

  public readonly createNewCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(callListActions.createNewCall),
      concatMap(({ name }) => this.callService.createNew(name).pipe(
        tap(({ callId }) => {
          this.router.navigate([`home/calls/${callId}`]);
        })
      ))
    )
  }, { dispatch: false }
  );

  public readonly joinCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(callListActions.joinCall),
      tap(({ callId }) => {
        this.router.navigate([`home/calls/${callId}`]);
      })
    )
  }, { dispatch: false });

  public readonly setListPage = createEffect(() =>
    this.actions$.pipe(
      ofType(callListActions.setListPage, callListActions.setPerPage, callListActions.addSortOption),
      map(() => callListActions.loadCalls()),
    )
  );

  public readonly loadCalls = createEffect(() =>
    this.actions$.pipe(
      ofType(callListActions.loadCalls),
      concatLatestFrom(() => this.facade.listConfig$),
      concatMap(([_, config]) =>
        this.callService.getAll(config.page, config.perPage, config.sortOptions).pipe(
          map(({calls, totalCount}) =>
            callListActions.loadCallsSuccess({ calls, totalCount })
          ),
          catchError((error) => of(callListActions.loadCallsFailure({ error }))),
        )
      )
    )
  );
}
