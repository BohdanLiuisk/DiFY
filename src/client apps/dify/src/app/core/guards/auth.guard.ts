import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '@core/services/auth/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  async canActivate(): Promise<boolean> {
    const authenticated = await this.authService.getIsAuthenticated();
    if(!authenticated) {
      await this.router.navigate(['start']);
    }
    return true;
  }
}
