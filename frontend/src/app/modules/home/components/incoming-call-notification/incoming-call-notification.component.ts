import { 
  Component, EventEmitter, Input, OnInit, Output 
} from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { IncomingCallEvent } from '@modules/home/models/dify.models';
import { GUID } from '@shared/custom-types';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-incoming-call-notification',
  templateUrl: './incoming-call-notification.component.html',
  styleUrls: ['./incoming-call-notification.component.scss'],
  providers: [MessageService]
})
export class IncomingCallNotificationComponent extends BaseComponent implements OnInit {
  public incomingCall: IncomingCallEvent = null;
  
  @Input('incomingCall')
  public incomingCallEvent: EventEmitter<IncomingCallEvent>;

  @Output('joinCall')
  public joinCallEvent = new EventEmitter<GUID>;

  @Output('declineCall')
  public declineCallEvent = new EventEmitter<GUID>;

  constructor(private toastService: MessageService) { 
    super();
  }

  public ngOnInit(): void {
    this.incomingCallEvent.pipe(this.untilThis).subscribe(incomingCall => {
      this.incomingCall = incomingCall;
      this.toastService.add({ 
        key: 'incomingCall', 
        sticky: true, 
        severity: 'info', 
        summary: 'Incoming call',
        closable: false 
      });
    });
  }

  public joinCall(): void {
    this.joinCallEvent.emit(this.incomingCall.id);
    this.toastService.clear('incomingCall');
  }

  public declineCall(): void {
    this.declineCallEvent.emit(this.incomingCall.id);
    this.toastService.clear('incomingCall');
  }
}
