import { User } from '@core/models/user';
import { GUID } from '@shared/custom-types';

export enum TokenStatus {
  PENDING = 'PENDING',
  VALIDATING = 'VALIDATING',
  VALID = 'VALID',
  INVALID = 'INVALID',
}

export interface AuthState {
  isLoggedIn: boolean;
  user?: User;
  accessTokenStatus: TokenStatus;
  refreshTokenStatus: TokenStatus;
  isLoadingLogin: boolean;
  hasLoginError: boolean;
}

export interface AuthUser {
  id: GUID;
  name: string,
  email: string,
}

export interface JwtToken {
  access_token: string,
  refresh_token: string
}

export interface LoginCredentials {
  username: string,
  password: string
}

export interface NewUser {
  login: string,
  password: string,
  email: string,
  firstName: string,
  lastName: string
}

export interface NewUserRegistered {
  id: GUID,
  login: string,
  password: string
}
