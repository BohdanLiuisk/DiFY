import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EMPTY, Observable, switchMap } from 'rxjs';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { JwtStorageService } from '@core/auth/jwt-storage.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { setBearerToHeader } from '@core/utils/http.utils';
import { dify } from '@shared/constans/app-settings';
import { JwtToken } from '@core/auth/store/auth.models';
import { AuthService } from '@core/auth/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authFacade: AuthFacade,
    private jwtStorage: JwtStorageService,
    private jwtHelper: JwtHelperService,
    private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(req.url.includes('userRegistrations') || req.url.includes('connect/token')) {
      return next.handle(req);
    }
    const jwt: JwtToken = {
      access_token: this.jwtStorage.getToken('access_token') || dify.emptyString,
      refresh_token: this.jwtStorage.getToken('refresh_token') || dify.emptyString
    }
    if(!Boolean(jwt.access_token)) {
      this.authFacade.logout();
      return EMPTY;
    }
    if(this.jwtHelper.isTokenExpired(jwt.access_token) && jwt.refresh_token) {
      return this.authService.refreshToken().pipe(
        switchMap(({ access_token, refresh_token}) => {
          this.jwtStorage.setToken({ key: 'access_token', value: access_token });
          this.jwtStorage.setToken({ key: 'refresh_token', value: refresh_token });
          this.authFacade.refreshTokenSuccess();
          return next.handle(setBearerToHeader(req, access_token));
        })
      )
    }
    return next.handle(setBearerToHeader(req, jwt.access_token));
  }
}
