import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable, takeUntil } from 'rxjs';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { IncomingCallNotificationComponent } from '../incoming-call-notification/incoming-call-notification.component';
import { IncomingCallNotification, MenuItem } from '@modules/home/models/dify.models';
import { DifyFacade } from '@modules/home/store/dify.facade';
import { AuthEventsService } from '@core/auth/services/auth-events.service';
import { DifySignalrEventsService } from '../../services/dify-signalr.events';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent implements OnInit {
  public currentUser$: Observable<AuthUser | undefined>;
  public sidebarOpened: boolean = false;
  public newCallNotification: Observable<boolean>;
  public menuItems: MenuItem[] = [];

  constructor(
    private authFacade: AuthFacade, 
    private difyFacade: DifyFacade,
    private difySignalrEvents: DifySignalrEventsService,
    private authEventsService: AuthEventsService,
    @Inject(Injector) private readonly injector: Injector,
    @Inject(TuiAlertService) private alertsService: TuiAlertService) {
    super()
  }

  public ngOnInit(): void {
    this.authEventsService.succesfullyAuthenticated$.pipe().subscribe(() => {
      this.difyFacade.onSuccessfullyAuthenticated();
    });
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

  public toogleMenu(): void {
    this.sidebarOpened = !this.sidebarOpened;
    let sidebar = document.querySelector(".sidebar");
    sidebar.classList.toggle("open");
    let mainNavigation = document.querySelector(".main-navigation");
    mainNavigation.classList.toggle("navigation-opened");
  }

  private setMenu(userId): void {
    this.menuItems = [
      {
        route: '/home',
        caption: 'Home',
        icon: 'home'
      },
      {
        route: `/home/profile/${userId}`,
        caption: 'My profile',
        icon: 'user'
      },
      {
        route: '/home/feed',
        caption: 'Feed',
        icon: 'news'
      },
      {
        route: '/home/friends',
        caption: 'Friends',
        icon: 'smile'
      },
      {
        route: '/home/call-history',
        caption: 'Call history',
        icon: 'phone-call'
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
          this.difyFacade.joinIncomingCall(incomingCall.callId);
        } else {
          this.difyFacade.declineIncomingCall(incomingCall.callId);
        }
      }
    });
  }
}
