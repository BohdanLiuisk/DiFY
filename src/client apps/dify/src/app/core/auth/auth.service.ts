import { APP_INITIALIZER, Injectable, Provider } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { environment } from '@env/environment';
import { Store } from '@ngrx/store';
import { AuthState, AuthUser, JwtToken, LoginCredentials, NewUser, TokenStatus } from './store/auth.models';
import * as AuthActions from './store/auth.actions';
import * as AuthSelectors from './store/auth.selectors';
import { filter, lastValueFrom, Observable, take } from 'rxjs';
import { dify, grandTypes } from '@shared/constans/app-settings';
import { HttpHeaders } from '@angular/common/http';
import { JwtStorageService } from '@core/auth/jwt-storage.service';
import { GUID } from '@shared/custom-types';

@Injectable()
export class AuthService {
  private readonly accessTokenPath: string = '/connect/token';
  private readonly userAccessPath: string = '/api/userAccess';
  private readonly signUpPath: string = '/userAccess/userRegistrations';
  private readonly clientId: string;
  private readonly clientSecret: string;

  constructor(private store: Store, private httpService: CoreHttpService, private jwtStorage: JwtStorageService) {
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
    return this.httpService.postRequest<JwtToken>(this.accessTokenPath, body.toString() as unknown as object,
      new HttpHeaders().set("Content-Type", "application/x-www-form-urlencoded;")
    );
  }

  public signUp(newUser: NewUser): Observable<{ newUserId: GUID }> {
    return this.httpService.postRequest<{ newUserId: GUID }>(this.signUpPath, newUser);
  }

  public confirmNewUser(newUserId: GUID): Observable<any> {
    return this.httpService.patchRequest<any>(`${this.signUpPath}/${newUserId}/confirm`);
  }

  public refreshToken(): Observable<JwtToken> {
    const body = this.getUrlEncodedBody();
    body.set('grant_type', grandTypes.refreshToken);
    const refreshToken: string = this.jwtStorage.getToken('refresh_token') || dify.emptyString;
    body.set('refresh_token', refreshToken);
    return this.httpService.postRequest<JwtToken>(this.accessTokenPath, body.toString() as unknown as object,
      new HttpHeaders().set("Content-Type", "application/x-www-form-urlencoded;")
    );
  }

  public getAuthUser(): Observable<AuthUser> {
    return this.httpService.getRequest(`${this.userAccessPath}/authUser`);
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
