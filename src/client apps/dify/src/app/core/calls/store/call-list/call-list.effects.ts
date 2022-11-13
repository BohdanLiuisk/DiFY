import { Injectable } from "@angular/core";
import { CallService } from '@core/calls/call.service';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { Actions, concatLatestFrom, createEffect, ofType } from '@ngrx/effects';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import { catchError, concatMap, map, of } from 'rxjs';

@Injectable()
export class CallListEffects {
  constructor(
    private actions$: Actions,
    private callService: CallService,
    private facade: CallListFacade,
  ) { }

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
