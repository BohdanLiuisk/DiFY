import { Injectable } from "@angular/core";
import { CallService } from '@core/calls/call.service';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { Actions, concatLatestFrom, createEffect, ofType } from '@ngrx/effects';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import { catchError, concatMap, map, of, tap } from 'rxjs';
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRoute, Router } from "@angular/router";

@Injectable()
export class CallListEffects {
  constructor(
    private router: Router,
    private actions$: Actions,
    private callService: CallService,
    private facade: CallListFacade,
    private snackBar: MatSnackBar
  ) { }

  public readonly createNewCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(callListActions.createNewCall),
      concatMap(({ name }) => this.callService.createNew(name).pipe(
        tap(({callId}) => {
          this.router.navigate([`home/calls/${callId}`]);
        })
      ))
    )
  },
  { dispatch: false }
  );

  public readonly joinCall = createEffect(() => {
    return this.actions$.pipe(
      ofType(callListActions.joinCall),
      concatMap(({ callId }) => this.callService.joinCall(callId).pipe(
        map(() => callListActions.joinCallSuccess({ callId })),
        catchError(error => of(callListActions.joinCallFailure({ error }))),
      )))
    }
  );

  public readonly joinCallSuccess= createEffect(() => {
    return this.actions$.pipe(
      ofType(callListActions.joinCallSuccess),
      tap(({ callId }) => {
        this.router.navigate([`home/calls/${callId}`]);
      })
    );
  }, { dispatch: false });

  public readonly joinCallFailure = createEffect(() => {
    return this.actions$.pipe(
      ofType(callListActions.joinCallFailure),
      tap(({ error: { error } }) => {
        this.snackBar.open(error.detail, 'Ok', {
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: ['error-snackbar'],
          duration: 2000
        });
      })
    );
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
