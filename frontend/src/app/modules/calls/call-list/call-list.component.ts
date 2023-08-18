import { ChangeDetectionStrategy, Component, Inject, Injector, OnInit } from '@angular/core';
import { Call, CallColumns } from '@core/calls/store/call-list/call-list.reducer';
import { BaseComponent } from '@core/components/base.component';
import { CallListFacade } from '@core/calls/store/call-list/call-list.facade';
import { GUID } from '@shared/custom-types';
import { CreateNewCallComponent } from '../create-new-call/create-new-call.component';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { CreateNewCallConfig } from '@core/calls/store/call-list/call-list.models';
import { filter } from 'rxjs';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CallListComponent extends BaseComponent implements OnInit {
  public readonly callColumns = CallColumns;
  public readonly pageSizeOptions: number[] = [5, 10, 25, 100];
  private readonly newCallDialog = this.dialogService.open<CreateNewCallConfig | null>(
    new PolymorpheusComponent(CreateNewCallComponent, this.injector), {
      dismissible: false,
      label: 'New call',
      size: 'm',
      closeable: false
    }
  );

  constructor(
    @Inject(TuiDialogService) private readonly dialogService: TuiDialogService,
    @Inject(Injector) private readonly injector: Injector,
    public callListFacade: CallListFacade) {
    super();
  }

  public ngOnInit(): void {
    this.callListFacade.setPage(1);
  }

  public joinCall(callId: GUID): void {
    this.callListFacade.joinCall(callId);
  }

  public openNewCallDialog(): void {
    this.newCallDialog.pipe(this.untilThis, filter(c => Boolean(c))).subscribe({
      next: (newCallConfig) => {
        this.callListFacade.createNew(newCallConfig);
      }
    });
  }

  public getCallStatusTag(call: Call): string {
    return call.active ? 'Active': 'Ended';
  }
}
