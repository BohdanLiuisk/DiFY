import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Call, CallColumns, CallList, SortDirection } from '@core/calls/store/call-list/call-list.reducer';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import {
  selectCallEntities,
  selectCallsTotalCount,
  selectIsLoading,
  selectSortOptions
} from '@core/calls/store/call-list/call-list.selectors';
import { filter, map, mergeMap, Observable, Subject } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent implements OnInit {
  public callsList$: Observable<Call[]> = this.store.select(selectCallEntities);
  public loading$: Observable<boolean> = this.store.select(selectIsLoading);
  public totalCount$: Observable<number> = this.store.select(selectCallsTotalCount);
  public sortOptions$ = this.store.select(selectSortOptions).pipe(
    mergeMap(options => options)
  );
  public pageSizeOptions: number[] = [5, 10, 25, 100]; 
  public pageSize: number = 10;
  public callColumns = CallColumns;
  public columnSorting: Subject<CallColumns> = new Subject<CallColumns>();

  constructor(private store: Store<CallList>) { }

  public ngOnInit(): void {
    this.setPage(0);
    this.columnSorting.subscribe(sortBy => {
      this.store.dispatch(callListActions.addSortOption({ sortBy }));
    })
  }

  public setPage(page: number): void {
    this.store.dispatch(callListActions.setListPage({ page: page + 1 }));
  }

  public setPerPage(perPage: number): void {
    this.store.dispatch(callListActions.setPerPage({ perPage }));
  }

  public paginatorClick(pageEvent: PageEvent): void {
    if(pageEvent.previousPageIndex !== pageEvent.pageIndex) {
      this.setPage(pageEvent.pageIndex);
    } else if(pageEvent.pageSize !== this.pageSize) {
      this.pageSize = pageEvent.pageSize;
      this.setPerPage(pageEvent.pageSize);
    }
  }

  public sortBy(column: CallColumns): void {
    this.store.dispatch(callListActions.addSortOption({ sortBy: column }));
  }

  public getSortIconByColumn(sortBy: CallColumns): Observable<string> {
    return this.sortOptions$.pipe(
      filter(option => option.column === sortBy),
      map(({ direction }) => this.getSortIcon(direction))
    );
  }

  private getSortIcon(direction: SortDirection): string { 
    if(direction === SortDirection.asc) {
      return 'expand_less';
    } else {
      return 'expand_more'
    }
  }
}
