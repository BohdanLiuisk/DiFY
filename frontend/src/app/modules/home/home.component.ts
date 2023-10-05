import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable, takeUntil } from 'rxjs';
import { Roles } from '@core/auth/roles';
import { Menu, MenuModes } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';
import { DifySignalrEvents } from '@core/auth/services/dify-signalr.events';import { Router } from '@angular/router';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { IncomingCallNotificationComponent } from '@core/components/incoming-call-notification/incoming-call-notification.component';
import { IncomingCallNotification } from '@core/auth/dify-app.models';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';

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
  public newCallNotification: Observable<boolean>;

  public sidebarModes = MenuModes;
  public roles = Roles;

  constructor(
    private authFacade: AuthFacade, 
    private callListFacade: CallListFacade,
    private difySignalrEvents: DifySignalrEvents,
    @Inject(Router) router: Router,
    @Inject(Injector) private readonly injector: Injector,
    @Inject(TuiAlertService) private alertsService: TuiAlertService) {
    super()
  }

  public ngOnInit(): void {
    this.authFacade.user$.pipe(this.untilThis, filterEmpty()).subscribe(user => {
      this.setMenu(user.id);
    });
    this.difySignalrEvents.incomingCallNotification$.pipe(this.untilThis).subscribe((incomingCall) => {
      this.showNewCallNotification(incomingCall);
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

  private showNewCallNotification(incomingCall: IncomingCallNotification): void {
    this.alertsService.open<boolean>( new PolymorpheusComponent(IncomingCallNotificationComponent, this.injector), {
      label: `Incoming call from ${incomingCall.callerName}`,
      data: incomingCall,
      status: TuiNotification.Info,
      autoClose: false,
      hasIcon: false
    })
    .pipe(takeUntil(this.difySignalrEvents.incomingCallNotification$))
    .subscribe({
      next: (join) => {
        if (join) {
          this.callListFacade.joinCall(incomingCall.callId);
        } else {
          this.callListFacade.declineIncomingCall(incomingCall.callId);
        }
      }
    });
  }
}
