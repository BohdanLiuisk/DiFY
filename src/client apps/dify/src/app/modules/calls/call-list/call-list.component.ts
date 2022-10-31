import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CallList } from '@core/calls/store/call-list/call-list.reducer';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import {
  selectCallEntities,
  selectCallsTotalCount,
  selectIsLoading
} from '@core/calls/store/call-list/call-list.selectors';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent implements OnInit {
  public readonly columns = [`name`, `status`, `actions`];
  public callsList$ = this.store.select(selectCallEntities);
  public loading$ = this.store.select(selectIsLoading);
  public totalCount$ = this.store.select(selectCallsTotalCount);

  constructor(private store: Store<CallList>) { }

  public ngOnInit(): void {
    this.setPage(0);
  }

  public setPage(page: number): void {
    this.store.dispatch(callListActions.setListPage({ page: page + 1 }));
  }

  public setPerPage(perPage: number): void {
    this.store.dispatch(callListActions.setPerPage({ perPage }));
  }

  public logEvent(event: any) {
    console.log(event);
  }
}
