import { createAction, props } from '@ngrx/store';
import { AuthUser, LoginCredentials, NewUser, NewUserRegistered } from '@core/auth/store/auth.models';
import { HttpErrorResponse } from '@angular/common/http';

export const loginRequest = createAction(
  '[Auth] Login Request',
  props<LoginCredentials>()
);
export const signUpRequest = createAction(
  '[Auth] Sign Up Request',
  props<NewUser>()
);
export const signUpFailure = createAction(
  '[Auth] Sign Up Failure',
  props<{ error: Error }>()
);
export const confirmNewUserRequest = createAction(
  '[Auth] Confirm New User Request',
  props<NewUserRegistered>()
);
export const newUserConfirmed = createAction(
  '[Auth] New New User Confirmed',
  props<NewUserRegistered>()
);
export const loginSuccess = createAction('[Auth] Login Success');
export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: HttpErrorResponse }>()
);
export const logout = createAction('[Auth] Logout');
export const getAuthUserRequest = createAction('[Auth] Auth User Request');
export const getAuthUserSuccess = createAction(
  '[Auth] Auth User Success',
  props<{ user: AuthUser }>()
);
export const getAuthUserFailure = createAction('[Auth] Auth User Failure');
export const refreshTokenRequest = createAction('[Auth] Refresh Token Request');
export const refreshTokenSuccess = createAction('[Auth] Refresh Token Success');
export const refreshTokenFailure = createAction('[Auth] Refresh Token Failure');
