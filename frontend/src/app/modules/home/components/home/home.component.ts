import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable, map, takeUntil } from 'rxjs';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';
import { TuiAlertService, TuiNotification } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { IncomingCallNotificationComponent } from '../incoming-call-notification/incoming-call-notification.component';
import { IncomingCallNotification, MenuItem } from '../../models/dify.models';
import { DifyFacade } from '../../store/dify.facade';
import { AuthEventsService } from '@core/auth/services/auth-events.service';
import { DifySignalrEventsService } from '../../services/dify-signalr.events';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent implements OnInit {
  public currentUser$: Observable<AuthUser | undefined>;
  public menuItems: MenuItem[] = [];

  constructor(
    public difyFacade: DifyFacade,
    private authFacade: AuthFacade, 
    private difySignalrEvents: DifySignalrEventsService,
    private authEventsService: AuthEventsService,
    @Inject(Injector) private readonly injector: Injector,
    @Inject(TuiAlertService) private alertsService: TuiAlertService,
    private themeService: ThemeService) {
    super();
  }

  public get wrapperThemeClasses(): Observable<any> {
    return this.difyFacade.layoutConfig$.pipe(map((config) => {
      return {
        'layout-dark': config.theme === 'dark',
        'layout-light': config.theme === 'light',
        'p-ripple-disabled': !config.ripple,
        'p-input-filled': config.inputFilled
      };
    }));
  }

  public get themeIcon(): Observable<string> {
    return this.difyFacade.theme$.pipe(map((theme) => {
      return theme === 'light' ? 'pi pi-moon': 'pi pi-sun';
    }));
  }

  public get themeIconClasses(): Observable<any> {
    return this.difyFacade.theme$.pipe(map((theme) => {
      return {
        'text-yellow-600': theme === 'dark',
        'text-blue-400': theme === 'light'
      };
    }));
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
    this.difyFacade.theme$.pipe(this.untilThis).subscribe(theme => {
      this.themeService.switchTheme(theme);
    });
  }

  public logout(): void {
    this.authFacade.logout();
  }

  private setMenu(userId: number): void {
    this.menuItems = [
      {
        route: '/home',
        caption: 'Home',
        icon: 'home',
        exact: true
      },
      {
        route: `/home/profile/${userId}`,
        caption: 'My profile',
        icon: 'user',
        exact: true
      },
      {
        route: '/home/feed',
        caption: 'Feed',
        icon: 'th-large',
        exact: true
      },
      {
        route: '/home/friends',
        caption: 'Friends',
        icon: 'users',
        exact: false
      },
      {
        route: '/home/call-history',
        caption: 'Call history',
        icon: 'phone',
        exact: false
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
