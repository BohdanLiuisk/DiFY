import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { filter, Observable, switchMap } from 'rxjs';
import { Injectable } from '@angular/core';
import { AuthService } from '@core/services/auth/auth.service';
import { setBearerToHeader } from '@core/utils/http.utils';

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(req.url.includes('userRegistrations') || req.url.includes('connect/token')) {
      return next.handle(req);
    }
    return this.authService.getJwtToken().pipe(
      filter(jwt => Boolean(jwt.access_token)),
      switchMap(jwt => {
        return next.handle(setBearerToHeader(req, jwt.access_token));
      })
    );
  }
}
