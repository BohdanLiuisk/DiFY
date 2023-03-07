import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, map, of } from 'rxjs';
import { UserProfileService } from '../user-profile.service';
import { userProfileActions } from './user-profile.actions';

@Injectable()
export class UserProfileEffects {
  constructor(
    private actions$: Actions,
    private userProfileService: UserProfileService
  ) { }

  public readonly loadUserProfile = createEffect(() => {
    return this.actions$.pipe(
      ofType(userProfileActions.loadProfile),
      exhaustMap(({ id }) => this.userProfileService.getUserProfile(id).pipe(
        map((user) => {
          return userProfileActions.loadProfileSuccess({ user });
        }),
        catchError((error) => of(userProfileActions.loadProfileFailed({ error })))
      ))
    )
  });
}
