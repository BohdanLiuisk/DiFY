import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, take, tap } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectIsLoggedIn } from '@core/auth/store/auth.selectors';

@Injectable({ providedIn: 'root' })
export class AuthGuard  {
  constructor(private store: Store, private router: Router) { }

  canActivate(): Observable<boolean> {
    return this.store.select(selectIsLoggedIn).pipe(
      take(1),
      tap(isLoggedIn => {
        if (!isLoggedIn) {
          this.router.navigate(['start']);
        }
      })
    );
  }
}
