import { Injectable } from "@angular/core";
import { GUID } from "@shared/custom-types";
import { Subject } from "rxjs";
import { CallConnectionData } from "./call/call.models";

@Injectable({ providedIn: 'root' })
export class CallSignalrEvents {
  public readonly callParticipantConnected = new Subject<CallConnectionData>();
  public readonly callParticipantConnected$ = this.callParticipantConnected.asObservable();
  public readonly participantLeft = new Subject<{ participantId: GUID }>();
  public readonly participantLeft$ = this.participantLeft.asObservable();
}
