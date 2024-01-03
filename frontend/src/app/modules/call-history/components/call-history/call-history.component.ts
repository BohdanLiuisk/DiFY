import { ChangeDetectionStrategy, Component, Inject, Injector, OnInit } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { CreateNewCallConfig } from '@modules/call-history/models/call-history.models';
import { CreateNewCallComponent } from '../create-new-call/create-new-call.component';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { TuiDialogService } from '@taiga-ui/core';
import { CallHistoryFacade } from '@modules/call-history/store/call-history.facade';
import { filter } from 'rxjs';

@Component({
  selector: 'app-call-history',
  templateUrl: './call-history.component.html',
  styleUrls: ['./call-history.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CallHistoryComponent extends BaseComponent implements OnInit {
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
    public callHistoryFacade: CallHistoryFacade) {
    super();
  }

  public ngOnInit(): void { }

  public openNewCallDialog(): void {
    this.newCallDialog.pipe(this.untilThis, filter(c => Boolean(c))).subscribe({
      next: (newCallConfig) => {
        this.callHistoryFacade.createNew(newCallConfig);
      }
    });
  }
}
