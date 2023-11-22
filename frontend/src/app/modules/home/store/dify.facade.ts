import { Store } from '@ngrx/store';
import { DifyState } from '../models/dify.models';
import { Injectable } from '@angular/core';
import { difyActions } from './dify.actions';
import { GUID } from '@shared/custom-types';

@Injectable()
export class DifyFacade {
  constructor(private store: Store<DifyState>) { }

  public onSuccessfullyAuthenticated(): void {
    this.store.dispatch(difyActions.successfullyAuthenticated());
  }

  public joinIncomingCall(callId: GUID) {
    this.store.dispatch(difyActions.joinIncomingCall({ callId }));
  }

  public declineIncomingCall(callId: GUID) {
    this.store.dispatch(difyActions.declineIncomingCall({ callId }));
  }
}
