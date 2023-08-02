import { Injectable } from "@angular/core";
import { Store } from '@ngrx/store';
import { Call, CallList, CallListConfig } from '@core/calls/store/call-list/call-list.reducer';
import { selectIsLoading, selectCallEntities, selectCallsTotalCount, selectListConfig } from './call-list.selectors';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import { Observable } from "rxjs";
import { GUID } from "@shared/custom-types";

@Injectable({ providedIn: 'root' })
export class CallListFacade {
  public callsList$: Observable<Call[]> = this.store.select(selectCallEntities);
  public loading$: Observable<boolean> = this.store.select(selectIsLoading);
  public listConfig$: Observable<CallListConfig> = this.store.select(selectListConfig);
  public totalCount$: Observable<number> = this.store.select(selectCallsTotalCount);

  constructor(private store: Store<CallList>) { }

  public setPage(page: number) {
    this.store.dispatch(callListActions.setListPage({ page }));
  }

  public setPerPage(perPage: number) {
    this.store.dispatch(callListActions.setPerPage({ perPage }));
  }

  public createNew(name: string) {
    this.store.dispatch(callListActions.createNewCall({ name }));
  }

  public joinCall(callId: GUID) {
    this.store.dispatch(callListActions.joinCall({ callId }));
  }
}
