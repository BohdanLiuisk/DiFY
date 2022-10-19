import { Inject, Injectable } from '@angular/core';
import * as AuthActions from './auth.actions';
import { AuthService } from '../auth.service';
import { JwtStorageService } from '@core/auth/jwt-storage.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, map, of, tap } from 'rxjs';
import { JwtToken } from '@core/auth/store/auth.models';
import { TuiAlertService } from '@taiga-ui/core/components/alert';
import { TuiNotification } from '@taiga-ui/core/enums/notification';

@Injectable()
export class AuthEffects {
  constructor(
    private router: Router,
    private actions$: Actions,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private jwtStorage: JwtStorageService,
    @Inject(TuiAlertService)
    private readonly alertService: TuiAlertService,
  ) { }

  signUp$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.signUpRequest),
      exhaustMap((newUser) =>
        this.authService.signUp(newUser).pipe(
          map(({ newUserId}) => {
            const { login, password  } = newUser;
            return AuthActions.confirmNewUserRequest({
              id: newUserId, login, password
            });
          }),
          catchError(error => of(AuthActions.signUpFailure({ error })))
        )
      )
    )
  });

  confirmNewUser$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.confirmNewUserRequest),
      exhaustMap((newUser) =>
        this.authService.confirmNewUser(newUser.id).pipe(
            map(() => {
              return AuthActions.newUserConfirmed(newUser);
            })
        )
      )
    )
  });

  newUserConfirmed$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.newUserConfirmed),
      map((newUser) =>
         AuthActions.loginRequest({ username: newUser.login, password: newUser.password })
      ),
      catchError((error) => of(AuthActions.signUpFailure({ error })))
    )
  });

  login$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.loginRequest),
      exhaustMap(credentials =>
        this.authService.signIn(credentials).pipe(
          map(jwtToken => {
            this.setToken(jwtToken);
            return AuthActions.loginSuccess();
          }),
          catchError(error => of(AuthActions.loginFailure({ error }))))
      )
    );
  });

  onLoginSuccess$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.loginSuccess),
      map(() => {
        this.router.navigate(['home']);
        return AuthActions.getAuthUserRequest();
      })
    );
  });

  refreshToken$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.refreshTokenRequest),
      exhaustMap(() =>
        this.authService.refreshToken().pipe(
          map(jwtToken => {
            this.setToken(jwtToken);
            return AuthActions.refreshTokenSuccess();
          }),
          catchError(() => of(AuthActions.refreshTokenFailure()))
        )
      )
    );
  });

  getUser$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.refreshTokenSuccess, AuthActions.getAuthUserRequest),
      exhaustMap(() =>
        this.authService.getAuthUser().pipe(
          map(user => AuthActions.getAuthUserSuccess({ user })),
          catchError(() => of(AuthActions.getAuthUserFailure()))
        )
      )
    );
  });

  loginTokenFailure$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(AuthActions.loginFailure),
      tap(({ error: { error: { error_description }} }) => {
        this.alertService.open(`Login failure`, {
          label: error_description,
          status: TuiNotification.Error }).subscribe();
      }));
    },
    { dispatch: false }
  );

  refreshTokenFailureOrLogout$ = createEffect(() => {
      return this.actions$.pipe(
        ofType(AuthActions.refreshTokenFailure, AuthActions.logout),
        tap(() => {
          this.jwtStorage.removeToken('access_token');
          this.jwtStorage.removeToken('refresh_token');
          this.router.navigate(['start']);
        })
      );
    },
    { dispatch: false }
  );

  signUpOrConfirmationFailure$ = createEffect(() => {
      return this.actions$.pipe(
        ofType(AuthActions.signUpFailure),
        tap(({ error }) => {
          //TODO: show modal
          console.log(error);
          this.router.navigate(['start']);
        })
      );
    },
    { dispatch: false }
  );

  private setToken(token: JwtToken): void {
    this.jwtStorage.setToken({
      key: 'access_token',
      value: token.access_token
    });
    this.jwtStorage.setToken({
      key: 'refresh_token',
      value: token.refresh_token
    });
  }
}
