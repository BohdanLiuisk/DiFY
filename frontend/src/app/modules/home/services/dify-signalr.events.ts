import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IncomingCallEvent } from '../models/dify.models';

@Injectable()
export class DifySignalrEventsService {
  public readonly incomingCallNotification = new Subject<IncomingCallEvent>();
  public readonly incomingCallNotification$ = this.incomingCallNotification.asObservable();
}
