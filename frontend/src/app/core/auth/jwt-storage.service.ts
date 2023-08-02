import { Injectable } from '@angular/core';
import { Token } from '@shared/custom-types';

@Injectable()
export class JwtStorageService {
  public getToken(key: Token): string | null {
    return localStorage.getItem(key);
  }

  public setToken(token: { key: Token, value: string }): void {
    localStorage.setItem(token.key, token.value);
  }

  public removeToken(key: Token): void {
    if(localStorage.getItem(key)) {
      localStorage.removeItem(key);
    }
  }
}
