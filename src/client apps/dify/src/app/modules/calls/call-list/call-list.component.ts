import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Call, CallColumns, CallList, CallListConfig, SortDirection, SortOption, SortOptions } from '@core/calls/store/call-list/call-list.reducer';
import { callListActions } from '@core/calls/store/call-list/call-list.actions';
import {
  selectCallEntities,
  selectCallsTotalCount,
  selectIsLoading,
  selectListConfig,
  selectSortOptions
} from '@core/calls/store/call-list/call-list.selectors';
import { filter, map, mergeMap, Observable, Subject, withLatestFrom } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent implements OnInit {
  public callsList$: Observable<Call[]> = this.store.select(selectCallEntities);
  public loading$: Observable<boolean> = this.store.select(selectIsLoading);
  public listConfig$: Observable<CallListConfig> = this.store.select(selectListConfig);
  public totalCount$: Observable<number> = this.store.select(selectCallsTotalCount);
  public sortOptions$ = this.store.select(selectSortOptions);
  public pageSizeOptions: number[] = [5, 10, 25, 100]; 
  public pageSize: number = 10;
  public callColumns = CallColumns;
  public columnSorting: Subject<CallColumns> = new Subject<CallColumns>();
  public pageEvent: Subject<PageEvent> = new Subject<PageEvent>();

  private sortColumnsSeqNumbers: Record<string, number> = {
    [CallColumns.startDate]: 3,
    [CallColumns.totalParticipants]: 2,
    [CallColumns.active]: 1
  }
  
  constructor(private store: Store<CallList>) { }

  public ngOnInit(): void {
    this.setPage(0);
    this.columnSorting.subscribe(sortBy => {
      this.store.dispatch(callListActions.addSortOption({ sortBy }));
    });
    this.pageEvent.pipe(
      filter(pageEvent => pageEvent.previousPageIndex !== pageEvent.pageIndex)
    ).subscribe(pageEvent => {
      this.setPage(pageEvent.pageIndex);
    });
    this.pageEvent.pipe(
      withLatestFrom(this.listConfig$),
      filter(([pageEvent, listConfig]) => pageEvent.pageSize !== listConfig.perPage),
      map(([pageEvent]) => pageEvent)
    ).subscribe(pageEvent => {
      this.setPerPage(pageEvent.pageSize);
    });
  }

  public setPage(page: number): void {
    this.store.dispatch(callListActions.setListPage({ page: page + 1 }));
  }

  public setPerPage(perPage: number): void {
    this.store.dispatch(callListActions.setPerPage({ perPage }));
  }

  public getSortIconByColumn(sortBy: CallColumns): Observable<string> {
    return this.sortOptions$.pipe(
      mergeMap(options => options),
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
