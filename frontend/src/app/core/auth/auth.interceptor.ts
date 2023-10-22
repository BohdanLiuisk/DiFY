import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { setBearerToHeader } from '@core/utils/http.utils';
import { AuthService } from '@core/auth/services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(req.url.includes('userRegistrations') || req.url.includes('connect/token')) {
      return next.handle(req);
    }
    return this.authService.getJwtToken().pipe(
      switchMap(({ access_token }) => next.handle(setBearerToHeader(req, access_token)))
    );
  }
}
