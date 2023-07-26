import { APP_INITIALIZER, Injectable, Provider } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { environment } from '@env/environment';
import { Store } from '@ngrx/store';
import { AuthState, AuthUser, JwtToken, LoginCredentials, NewUser, TokenStatus } from './store/auth.models';
import * as AuthActions from './store/auth.actions';
import * as AuthSelectors from './store/auth.selectors';
import { EMPTY, filter, lastValueFrom, map, Observable, of, take } from 'rxjs';
import { dify, grandTypes } from '@shared/constans/app-settings';
import { HttpHeaders } from '@angular/common/http';
import { JwtStorageService } from '@core/auth/jwt-storage.service';
import { GUID } from '@shared/custom-types';
import { JwtHelperService } from '@auth0/angular-jwt';
import { QueryResponse } from '@core/models/query-response';
import { User } from '@core/models/user';

@Injectable()
export class AuthService {
  private readonly accessTokenPath: string = '/connect/token';
  private readonly usersApi: string = '/api/users';
  private readonly signUpPath: string = '/userAccess/userRegistrations';
  private readonly clientId: string;
  private readonly clientSecret: string;

  constructor(
    private store: Store,
    private httpService: CoreHttpService,
    private jwtStorage: JwtStorageService,
    private jwtHelper: JwtHelperService) {
    this.clientId = environment.authConfig.client_id;
    this.clientSecret = environment.authConfig.client_secret;
  }

  public init(): Promise<AuthState> {
    if(!this.jwtStorage.getToken('refresh_token')) {
      return Promise.resolve<AuthState>(null);
    }
    this.store.dispatch(AuthActions.refreshTokenRequest());
    const authState$ = this.store.select(AuthSelectors.selectAuth).pipe(
      filter(auth => auth.refreshTokenStatus === TokenStatus.INVALID ||
        (auth.refreshTokenStatus === TokenStatus.VALID && !!auth.user)),
      take(1)
    );
    return lastValueFrom(authState$);
  }

  public signIn(credentials: LoginCredentials): Observable<JwtToken> {
    const body = this.getUrlEncodedBody();
    body.set('grant_type', grandTypes.password);
    body.set('username', credentials.username);
    body.set('password', credentials.password);
    return this.httpService.postRequest<JwtToken>(this.accessTokenPath, body.toString() as unknown as object,{ 
      headers: new HttpHeaders().set("Content-Type", "application/x-www-form-urlencoded;"), 
      isQueryResponse: false 
    });
  }

  public signUp(newUser: NewUser): Observable<{ newUserId: GUID }> {
    return this.httpService.postRequest<{ newUserId: GUID }>(this.signUpPath, newUser);
  }

  public confirmNewUser(newUserId: GUID): Observable<any> {
    return this.httpService.patchRequest<any>(`${this.signUpPath}/${newUserId}/confirm`);
  }

  public getJwtToken(): Observable<JwtToken> {
    const jwt: JwtToken = {
      access_token: this.jwtStorage.getToken('access_token') ?? dify.emptyString,
      refresh_token: this.jwtStorage.getToken('refresh_token') ?? dify.emptyString
    }
    if(!Boolean(jwt.access_token)) {
      this.store.dispatch(AuthActions.logout());
      return EMPTY;
    }
    if(this.jwtHelper.isTokenExpired(jwt.access_token) && jwt.refresh_token) {
      return this.refreshToken().pipe(
        map(({ access_token, refresh_token }) => {
          this.jwtStorage.setToken({ key: 'access_token', value: access_token });
          this.jwtStorage.setToken({ key: 'refresh_token', value: refresh_token });
          this.store.dispatch(AuthActions.refreshTokenSuccess());
          return { access_token, refresh_token };
        })
      )
    }
    return of(jwt);
  }

  public refreshToken(): Observable<JwtToken> {
    const body = this.getUrlEncodedBody();
    body.set('grant_type', grandTypes.refreshToken);
    const refreshToken: string = this.jwtStorage.getToken('refresh_token') || dify.emptyString;
    body.set('refresh_token', refreshToken);
    return this.httpService.postRequest<JwtToken>(this.accessTokenPath, body.toString() as unknown as object, {
      headers: new HttpHeaders().set("Content-Type", "application/x-www-form-urlencoded;"),
      isQueryResponse: false
    });
  }

  public getAuthUser(): Observable<User> {
    return this.httpService.getRequest<User>(`${this.usersApi}/getCurrentUser`);
  }

  private getUrlEncodedBody(): URLSearchParams {
    const body = new URLSearchParams();
    body.set('client_id', this.clientId);
    body.set('client_secret', this.clientSecret);
    return body;
  }
}

export const authServiceInitProvider: Provider = {
  provide: APP_INITIALIZER,
  useFactory: (authService: AuthService) => () => authService.init(),
  deps: [AuthService],
  multi: true,
};
