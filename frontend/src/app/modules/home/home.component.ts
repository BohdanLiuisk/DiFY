import { Component, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable } from 'rxjs';
import { Roles } from '@core/auth/roles';
import { Menu, MenuModes } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';

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
    this.authFacade.user$.pipe(this.untilThis, filterEmpty()).subscribe(user => {
      this.setMenu(user.id);
    });
  }

  public logout(): void {
    this.authFacade.logout();
  }

  private setMenu(userId): void {
    this.menu = [
      {
        id: 'HOME',
        label: 'Home',
        route: '/home',
        iconClass: 'tuiIconHome',
        linkActiveExact: true
      },
      {
        id: 'SOCIAL',
        header: 'Social',
      },
      {
        id: 'my_profile',
        label: 'My profile',
        route: `social/profile/${userId}`,
        iconClass: 'tuiIconUser',
        linkActiveExact: false
      },
      {
        id: 'Feed',
        label: 'Feed',
        route: 'social',
        iconClass: 'tuiIconFileText',
        linkActiveExact: true
      },
      {
        id: 'friends',
        label: 'Friends',
        route: 'social/friends',
        iconClass: 'tuiIconUsers',
        linkActiveExact: true
      },
      {
        id: 'CALLS',
        label: 'Calls',
        route: 'social/calls',
        iconClass: 'tuiIconPhone',
        linkActiveExact: false,
      }
    ];
  }
}
