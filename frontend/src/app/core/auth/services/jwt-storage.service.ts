import { Injectable } from '@angular/core';
import { Token } from '@shared/custom-types';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class JwtStorageService {
  constructor(private cookieService: CookieService) {}

  public getToken(key: Token): string | null {
    return this.cookieService.get(key) || null;
  }

  public setToken(token: { key: Token, value: string }): void {
    this.cookieService.set(token.key, token.value, {
      path: '/',
      secure: true,
      sameSite: 'Strict'
    });
  }

  public removeToken(key: Token): void {
    this.cookieService.delete(key, '/');
  }
}
