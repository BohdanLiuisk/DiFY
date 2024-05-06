import { Component, EventEmitter, OnInit } from '@angular/core';
import { AuthUser } from '@core/auth/store/auth.models';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { Observable, map } from 'rxjs';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';
import { IncomingCallEvent, MenuItem } from '../../models/dify.models';
import { DifyFacade } from '../../store/dify.facade';
import { AuthEventsService } from '@core/auth/services/auth-events.service';
import { DifySignalrEventsService } from '../../services/dify-signalr.events';
import { ThemeService } from '../../services/theme.service';
import { GUID } from '@shared/custom-types';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent implements OnInit {
  public currentUser$: Observable<AuthUser | undefined>;
  public menuItems: MenuItem[] = [];
  public readonly incomingCallEvent: EventEmitter<IncomingCallEvent> = 
    new EventEmitter();

  constructor(
    public difyFacade: DifyFacade,
    private authFacade: AuthFacade, 
    private difySignalrEvents: DifySignalrEventsService,
    private authEventsService: AuthEventsService,
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
      this.incomingCallEvent.emit(incomingCall);
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

  public joinIncomingCall(callId: GUID): void {
    this.difyFacade.joinCall(callId);
  }

  public declineIncomingCall(callId: GUID): void {
    this.difyFacade.declineIncomingCall(callId);
  }
}
