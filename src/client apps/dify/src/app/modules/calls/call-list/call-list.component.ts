import { Component, Inject, Injector, OnInit } from '@angular/core';
import { Call, CallColumns, SortDirection } from '@core/calls/store/call-list/call-list.reducer';
import { filter, map, Observable, Subject, withLatestFrom } from 'rxjs';
import { BaseComponent } from '@core/components/base.component';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { CreateNewCallComponent } from '../create-new-call/create-new-call.component';
import { GUID } from '@shared/custom-types';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent extends BaseComponent implements OnInit {
  public readonly callColumns = CallColumns;
  public readonly pageSizeOptions: number[] = [5, 10, 25, 100];
  public readonly columnSorting: Subject<CallColumns> = new Subject<CallColumns>();
  public newCallOpened: boolean = false;

  private readonly sortColumnsSeqNumbers: Record<string, number> = {
    [CallColumns.startDate]: 3,
    [CallColumns.totalParticipants]: 2,
    [CallColumns.active]: 1
  }

  private readonly newCallDialog = this.dialogService.open<{ name: string }>(
    new PolymorpheusComponent(CreateNewCallComponent, this.injector), {
      dismissible: true,
      label: 'New call',
      size: 'm'
    }
  );

  constructor(
    public callListFacade: CallListFacade,
    @Inject(TuiDialogService) private readonly dialogService: TuiDialogService,
    @Inject(Injector) private readonly injector: Injector,) {
    super();
  }

  public ngOnInit(): void {
    this.callListFacade.setPage(1);
    this._subscribeColumnSorting();
  }

  public getSortIconByColumn(sortBy: CallColumns): Observable<string> {
    return this.callListFacade.sortOptions$.pipe(
      map(options => options.find(option => option.column === sortBy)),
      filter(option => !!option),
      map(({ direction }) => this._getSortIcon(direction))
    );
  }

  public createNew(): void {
    this.newCallDialog.pipe(this.untilThis).subscribe({
      next: ({ name }) => this.callListFacade.createNew(name)
    });
  }

  public joinCall(callId: GUID): void {
    this.callListFacade.joinCall(callId);
  }

  public getCallStatusTag(call: Call): string {
    return call.active ? 'Active': 'Ended';
  }

  private _getSortIcon(direction: SortDirection): string {
    if(direction === SortDirection.asc) {
      return 'tuiIconChevronUp';
    } else {
      return 'tuiIconChevronDown';
    }
  }

  private _subscribeColumnSorting(): void {
    this.columnSorting.pipe(
      this.untilThis,
      withLatestFrom(this.callListFacade.sortOptions$),
      map(([sortBy, sortOptions]) => {
        const existingOption = sortOptions.find(option => option.column === sortBy);
        const direction = existingOption ? this._getOppositeSortDirection(existingOption.direction) : SortDirection.asc;
        return {
          column: sortBy,
          direction,
          seqNumber: this.sortColumnsSeqNumbers[sortBy]
        };
      })
    ).subscribe(sortOption => {
      this.callListFacade.addSortOption(sortOption);
    });
  }

  private _getOppositeSortDirection(direction: SortDirection): SortDirection {
    return direction === SortDirection.asc ? SortDirection.desc : SortDirection.asc;
  }
}
