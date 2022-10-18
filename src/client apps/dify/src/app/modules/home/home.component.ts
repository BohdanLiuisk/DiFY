import { Component, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { Router } from '@angular/router';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public currentUser$: Observable<AuthUser | undefined>;

  constructor(private authFacade: AuthFacade, private router: Router) { }

  ngOnInit(): void {
    this.currentUser$ = this.authFacade.user$;
  }

  public async back(): Promise<void> {
    await this.router.navigate(['start']);
  }

  public logout(): void {
    this.authFacade.logout();
  }
}
