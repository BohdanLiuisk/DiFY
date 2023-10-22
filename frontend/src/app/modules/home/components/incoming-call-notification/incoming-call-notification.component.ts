import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { IncomingCallNotification } from '@modules/home/store/dify.models';
import { TuiDialog } from '@taiga-ui/cdk';
import { TuiAlertOptions } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';

@Component({
  selector: 'app-incoming-call-notification',
  templateUrl: './incoming-call-notification.component.html',
  styleUrls: ['./incoming-call-notification.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class IncomingCallNotificationComponent {
  public value: IncomingCallNotification;

  constructor(
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly context: TuiDialog<TuiAlertOptions<IncomingCallNotification>, boolean>) {
    this.value = this.context.data;
  }

  public confirmJoin(value: boolean): void {
    this.context.completeWith(value);
  }
}
