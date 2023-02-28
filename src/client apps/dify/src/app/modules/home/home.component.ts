import { Component, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable } from 'rxjs';
import { Roles } from '@core/auth/roles';
import { Menu, MenuModes } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { BaseComponent } from '@core/components/base.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent implements OnInit {
  public currentUser$: Observable<AuthUser | undefined>;
  public menu: Menu;
  public currentRole = Roles.ADMIN;
  public sidebarCollapsed: boolean = false;
  public currentSearch?: string;
  public inputSearchFocus: boolean = false;
  public mainNavigationOpened: boolean = true;

  public sidebarModes = MenuModes;
  public roles = Roles;

  constructor(private authFacade: AuthFacade) {
    super()
  }

  public ngOnInit(): void {
    this.currentUser$ = this.authFacade.user$.pipe(this.untilThis);
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
        iconClass: 'tuiIconHome'
      },
      {
        id: 'CALLS',
        label: 'Calls',
        route: 'calls',
        iconClass: 'tuiIconPhone',
        linkActiveExact: false,
      }
    ];
  }
}
