import { Injectable } from "@angular/core";
import { GUID } from "@shared/custom-types";
import { Subject } from "rxjs";
import { Participant } from "./call/call.models";

@Injectable({ providedIn: 'root' })
export class CallSignalrEvents {
  public readonly callParticipantConnected = new Subject<Participant>();
  public readonly callParticipantConnected$ = this.callParticipantConnected.asObservable();
  public readonly participantLeft = new Subject<{ participantId: GUID }>();
  public readonly participantLeft$ = this.participantLeft.asObservable();
  public readonly updateVideoTrack = new Subject<{ videoTrack: MediaStreamTrack }>();
  public readonly updateVideoTrack$ = this.updateVideoTrack.asObservable();;
}
