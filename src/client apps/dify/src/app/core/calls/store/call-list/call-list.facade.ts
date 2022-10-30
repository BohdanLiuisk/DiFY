import { Injectable } from "@angular/core";
import { Store } from '@ngrx/store';
import { CallList } from '@core/calls/store/call-list/call-list.reducer';
import { selectIsLoading, selectCallEntities, selectCallsTotalCount, selectListConfig } from './call-list.selectors';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';

@Injectable({ providedIn: 'root' })
export class CallListFacade {
  public listConfig$ = this.store.select(selectListConfig);
  public calls$ = this.store.select(selectCallEntities);
  public callsTotalCount$ = this.store.select(selectCallsTotalCount);
  public isLoading$ = this.store.select(selectIsLoading);

  constructor(private store: Store<CallList>) { }

  public setPage(page: number) {
    this.store.dispatch(callListActions.setListPage({ page }));
  }
}
