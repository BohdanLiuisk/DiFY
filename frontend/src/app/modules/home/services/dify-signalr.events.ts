import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IncomingCallNotification } from '../store/dify.models';

@Injectable()
export class DifySignalrEventsService {
  public readonly incomingCallNotification = new Subject<IncomingCallNotification>();
  public readonly incomingCallNotification$ = this.incomingCallNotification.asObservable();
}
