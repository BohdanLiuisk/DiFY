import { Component, OnInit } from '@angular/core';
import { CallColumns, SortDirection } from '@core/calls/store/call-list/call-list.reducer';
import { filter, map, mergeMap, Observable, Subject, withLatestFrom } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { BaseComponent } from '@core/components/base.component';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { Dialog } from '@angular/cdk/dialog';
import { CreateNewCallComponent } from '../create-new-call/create-new-call.component';
import { GUID } from '@shared/custom-types';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent extends BaseComponent implements OnInit {
  public callColumns = CallColumns;
  public pageSizeOptions: number[] = [5, 10, 25, 100];
  public columnSorting: Subject<CallColumns> = new Subject<CallColumns>();
  public pageEvent: Subject<PageEvent> = new Subject<PageEvent>();

  private readonly sortColumnsSeqNumbers: Record<string, number> = {
    [CallColumns.startDate]: 3,
    [CallColumns.totalParticipants]: 2,
    [CallColumns.active]: 1
  }

  constructor(public callListFacade: CallListFacade, private _dialog: Dialog) {
    super();
  }

  public ngOnInit(): void {
    this.callListFacade.setPage(1);
    this._subscribeColumnSorting();
    this._subscribePagination();
  }

  public getSortIconByColumn(sortBy: CallColumns): Observable<string> {
    return this.callListFacade.sortOptions$.pipe(
      mergeMap(options => options),
      filter(option => option.column === sortBy),
      map(({ direction }) => this.getSortIcon(direction))
    );
  }

  public createNew(): void {
    const dialogRef = this._dialog.open<{ name: string }>(CreateNewCallComponent, {
      disableClose: true,
      autoFocus: true,
      restoreFocus: false,
      width: '370px'
    });
    dialogRef.closed.subscribe(result => {
      if(result && result.name) {
        this.callListFacade.createNew(result.name);
      }
    });
  }

  public joinCall(callId: GUID): void {
    this.callListFacade.joinCall(callId);
  }

  private getSortIcon(direction: SortDirection): string {
    if(direction === SortDirection.asc) {
      return 'expand_less';
    } else {
      return 'expand_more';
    }
  }

  private _subscribePagination(): void {
    this.pageEvent.pipe(
      this.untilThis,
      filter(pageEvent => pageEvent.previousPageIndex !== pageEvent.pageIndex)
    ).subscribe(({ pageIndex: page } ) => {
      this.callListFacade.setPage(page + 1);
    });
    this.pageEvent.pipe(
      this.untilThis,
      withLatestFrom(this.callListFacade.listConfig$),
      filter(([pageEvent, listConfig]) => pageEvent.pageSize !== listConfig.perPage),
      map(([pageEvent]) => pageEvent)
    ).subscribe(({ pageSize: perPage }) => {
      this.callListFacade.setPerPage(perPage);
    });
  }

  private _subscribeColumnSorting(): void {
    this.columnSorting.pipe(
      this.untilThis,
      withLatestFrom(this.callListFacade.sortOptions$),
      map(([sortBy, sortOptions]) => {
        const existingOption = sortOptions.filter(sortOption => sortOption.column === sortBy)[0];
        if(existingOption) {
          return {
            ...existingOption,
            direction: this._getOppositeSortDirection(existingOption.direction)
          };
        } else {
          return {
            column: sortBy,
            direction: SortDirection.asc,
            seqNumber: this.sortColumnsSeqNumbers[sortBy]
          };
        }
      })
    ).subscribe(sortOption => {
      this.callListFacade.addSortOption(sortOption);
    });
  }

  private _getOppositeSortDirection(direction: SortDirection): SortDirection {
    return direction === SortDirection.asc ? SortDirection.desc : SortDirection.asc;
  }
}
