import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap, takeUntil } from 'rxjs';
import { userProfileActions } from './user-profile.actions';
import { UserProfileService } from '../services/user-profile.service';

@Injectable()
export class UserProfileEffects {
  constructor(
    private actions$: Actions,
    private userProfileService: UserProfileService
  ) { }

  public readonly loadUserProfile = createEffect(() => {
    return this.actions$.pipe(
      ofType(userProfileActions.loadProfile),
      switchMap(({ id }) => this.userProfileService.getUserProfile(id).pipe(
        map((user) => {
          return userProfileActions.loadProfileSuccess({ user });
        }),
        catchError((error) => {
          return of(userProfileActions.loadProfileFailed({ error }));
        }),
        takeUntil(this.actions$.pipe(ofType(userProfileActions.clearState)))
      ))
    )
  });
}
