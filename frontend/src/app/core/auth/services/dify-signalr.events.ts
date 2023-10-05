import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { IncomingCallNotification } from "../dify-app.models";

@Injectable({ providedIn: 'root' })
export class DifySignalrEvents {
  public readonly incomingCallNotification = new Subject<IncomingCallNotification>();
  public readonly incomingCallNotification$ = this.incomingCallNotification.asObservable();
}
