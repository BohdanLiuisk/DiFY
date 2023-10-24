import { 
    selectCallEntities, 
    selectCallsTotalCount, 
    selectHistoryConfig, 
    selectIsLoading, 
    selectNewCallCreating 
} from './call-history.selectors';
import { Observable } from 'rxjs';
import { Call, CallHistory, CallHistoryConfig, CreateNewCallConfig } from '../models/call-history.models';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { callHistoryActions } from './call-history.actions';
import { GUID } from '@shared/custom-types';

@Injectable()
export class CallHistoryFacade {
  public callsHistory$: Observable<Call[]> = this.store.select(selectCallEntities);
  public loading$: Observable<boolean> = this.store.select(selectIsLoading);
  public historyConfig$: Observable<CallHistoryConfig> = this.store.select(selectHistoryConfig);
  public totalCount$: Observable<number> = this.store.select(selectCallsTotalCount);
  public isCallCreating$: Observable<boolean> = this.store.select(selectNewCallCreating);

  constructor(private store: Store<CallHistory>) { }

  public setPage(page: number) {
    this.store.dispatch(callHistoryActions.setHistoryPage({ page }));
  }

  public setPerPage(perPage: number) {
    this.store.dispatch(callHistoryActions.setPerPage({ perPage }));
  }

  public createNew(newCallConfig: CreateNewCallConfig) {
    this.store.dispatch(callHistoryActions.createNewCall(newCallConfig));
  }

  public joinCall(callId: GUID) {
    this.store.dispatch(callHistoryActions.joinCall({ callId }));
  }
}
