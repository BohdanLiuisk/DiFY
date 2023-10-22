import { Injectable } from '@angular/core';
import * as AuthActions from '@core/auth/store/auth.actions';
import * as AuthSelectors from '@core/auth/store/auth.selectors';
import { Store } from '@ngrx/store';
import { NewUser } from '@core/auth/store/auth.models';
import { filterEmpty } from '@core/utils/pipe.operators';

@Injectable()
export class AuthFacade {
  public auth$ = this.store.select(AuthSelectors.selectAuth);
  public user$ = this.store.select(AuthSelectors.selectAuthUser);
  public userId$ = this.store.select(AuthSelectors.selectAuthUserId).pipe(filterEmpty());
  public isLoggedIn$ = this.store.select(AuthSelectors.selectIsLoggedIn);
  public isLoadingLogin$ = this.store.select(AuthSelectors.selectIsLoadingLogin);
  public hasLoginError$ = this.store.select(AuthSelectors.selectLoginError);

  constructor(private store: Store) { }

  public signUp(newUser: NewUser): void {
    this.store.dispatch(AuthActions.signUpRequest(newUser));
  }

  public login(username: string, password: string): void {
    this.store.dispatch(AuthActions.loginRequest({ username, password }));
  }

  public logout(): void {
    this.store.dispatch(AuthActions.logout());
  }

  public getAuthUser(): void {
    this.store.dispatch(AuthActions.getAuthUserRequest());
  }

  public refreshTokenSuccess(): void {
    this.store.dispatch(AuthActions.refreshTokenSuccess());
  }
}
