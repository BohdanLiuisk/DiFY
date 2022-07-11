import { CoreHttpService } from '@core/services/core-http.service';
import { LoginCredentials } from '@core/models/auth/login.credentials';
import { JwtStorageService } from '@core/services/auth/jwt-storage.service';
import { filter, lastValueFrom, Observable, of, tap } from 'rxjs';
import { JwtToken } from '@core/models/auth/jwt.token';
import { User } from '@core/models/user/user.model';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { dify, grandTypes } from '@shared/constans/app-settings';
import { HttpHeaders } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly accessTokenPath: string = '/connect/token';
  private readonly userAccessPath: string = '/api/userAccess';
  private readonly clientId: string = 'ro.client';
  private readonly clientSecret: string = 'secret';

  constructor(
    private httpService: CoreHttpService,
    private jwtStorage: JwtStorageService,
    private router: Router,
    private jwtHelper: JwtHelperService) { }

  public signIn(credentials: LoginCredentials): Observable<JwtToken> {
    const body = this.getUrlEncodedBody();
    body.set('grant_type', grandTypes.password);
    body.set('username', credentials.username);
    body.set('password', credentials.password);
    return this.getToken(body).pipe(
      tap(async token => {
        this.setToken(token);
        await this.router.navigate(['home']);
      })
    );
  }

  public getAuthUser(): Observable<User> {
    return this.httpService.getRequest(`${this.userAccessPath}/authUser`);
  }

  public getJwtToken(): Observable<JwtToken> {
    const jwt: JwtToken = {
      access_token: this.jwtStorage.getToken('access_token') || dify.emptyString,
      refresh_token: this.jwtStorage.getToken('refresh_token') || dify.emptyString
    }
    if(!jwt.access_token) {
      this.router.navigate(['start']);
      return of(jwt);
    }
    if(this.jwtHelper.isTokenExpired(jwt.access_token) && jwt.refresh_token) {
      return this.getRefreshedToken().pipe(
        tap(async token => {
          this.setToken(token);
        })
      );
    }
    return of(jwt);
  }

  public async getIsAuthenticated(): Promise<boolean> {
    return Boolean(await lastValueFrom(this.getJwtToken().pipe(
      filter(jwt => Boolean(jwt.access_token))
    )));
  }

  public getRefreshedToken(): Observable<JwtToken> {
    const body = this.getUrlEncodedBody();
    body.set('grant_type', grandTypes.refreshToken);
    const refreshToken: string = this.jwtStorage.getToken('refresh_token') || dify.emptyString;
    body.set('refresh_token', refreshToken);
    return this.getToken(body);
  }

  private getToken(body: URLSearchParams): Observable<JwtToken> {
    return this.httpService.postRequest<JwtToken>(
      this.accessTokenPath,
      body.toString() as unknown as object,
      new HttpHeaders().set(
        "Content-Type",
        "application/x-www-form-urlencoded;"
      )).pipe(filter(token => Boolean(token)));
  }

  public setToken(token: JwtToken): void {
    this.jwtStorage.setToken({
      key: 'access_token',
      value: token.access_token
    });
    this.jwtStorage.setToken({
      key: 'refresh_token',
      value: token.refresh_token
    });
  }

  private getUrlEncodedBody(): URLSearchParams {
    const body = new URLSearchParams();
    body.set('client_id', this.clientId);
    body.set('client_secret', this.clientSecret);
    return body;
  }
}
