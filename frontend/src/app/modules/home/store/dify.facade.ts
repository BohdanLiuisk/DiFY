import { Store } from '@ngrx/store';
import { DifyState, LayoutConfig } from '../models/dify.models';
import { Injectable } from '@angular/core';
import { difyActions } from './dify.actions';
import { GUID, Theme } from '@shared/custom-types';
import { Observable } from 'rxjs';
import { selectLayoutConfig, selectSidebarOpened, selectTheme } from './dify.selectors';

@Injectable()
export class DifyFacade {
  public sidebarOpened$: Observable<boolean> = this.store.select(selectSidebarOpened);
  public layoutConfig$: Observable<LayoutConfig> = this.store.select(selectLayoutConfig);
  public theme$: Observable<Theme> = this.store.select(selectTheme);

  constructor(private store: Store<DifyState>) { }

  public onSuccessfullyAuthenticated(): void {
    this.store.dispatch(difyActions.successfullyAuthenticated());
  }

  public joinIncomingCall(callId: GUID): void {
    this.store.dispatch(difyActions.joinIncomingCall({ callId }));
  }

  public declineIncomingCall(callId: GUID): void {
    this.store.dispatch(difyActions.declineIncomingCall({ callId }));
  }

  public toggleSidebar(): void {
    this.store.dispatch(difyActions.toggleSidebar());
  }

  public switchTheme(): void {
    this.store.dispatch(difyActions.switchTheme());
  }
}
