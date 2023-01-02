import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { Router } from '@angular/router';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable } from 'rxjs';
import { Roles } from '@core/auth/roles';
import { Menu, MenuModes } from '@shared/modules/sidebar-menu/sidebar-menu.types';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public currentUser$: Observable<AuthUser | undefined>;
  public menu: Menu;
  public currentRole = Roles.ADMIN;
  public sidebarCollapsed: boolean = false;
  public currentSearch?: string;
  public inputSearchFocus: boolean = false;
  public mainNavigationOpened: boolean = true;

  public sidebarModes = MenuModes;
  public roles = Roles;

  constructor(private authFacade: AuthFacade, private router: Router) { }

  public ngOnInit(): void {
    this.currentUser$ = this.authFacade.user$;
    this.setMenu();
  }

  public logout(): void {
    this.authFacade.logout();
  }

  private setMenu(): void {
    this.menu = [
      {
        id: 'SOCIAL',
        header: 'Social',
      },
      {
        id: 'HOME',
        label: 'Home',
        route: '/home',
        iconClass: 'fa fa-home',
      },
      {
        id: 'CALLS',
        label: 'Calls',
        route: 'calls',
        iconClass: 'fa fa-phone',
        linkActiveExact: false
      }
    ];
  }
}
