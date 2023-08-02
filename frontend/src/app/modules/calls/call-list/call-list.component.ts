import { Component, Inject, Injector, OnInit } from '@angular/core';
import { Call, CallColumns } from '@core/calls/store/call-list/call-list.reducer';
import { BaseComponent } from '@core/components/base.component';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { CreateNewCallComponent } from '../create-new-call/create-new-call.component';
import { GUID } from '@shared/custom-types';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { Router } from '@angular/router';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent extends BaseComponent implements OnInit {
  public readonly callColumns = CallColumns;
  public readonly pageSizeOptions: number[] = [5, 10, 25, 100];
  public newCallOpened: boolean = false;

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
    @Inject(Injector) private readonly injector: Injector,
    private router: Router) {
    super();
  }

  public ngOnInit(): void {
    this.callListFacade.setPage(1);
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

  public addQueryFiltrationParam(section) {
    this.router.navigate(['/home/social/calls'], { queryParams: { section } });
  }
}
