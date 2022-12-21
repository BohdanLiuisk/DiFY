import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { filter, Observable } from "rxjs";
import { callActions } from '@core/calls/store/call/call.actions';
import {
  selectCall,
  selectCallId,
  selectConnectionData,
  selectCurrentStreamId,
  selectLoaded,
  selectLoading,
  selectMediaTracks,
  selectTestMessage
} from "@core/calls/store/call/call.selectors";
import { Call, CallConnectionData, CallState } from "@core/calls/store/call/call.models";
import { stopSignalRHub } from "ngrx-signalr-core";
import { callHub } from "./call.hub";

@Injectable({ providedIn: 'root' })
export class CallFacade {
  public readonly callId$: Observable<GUID> = this.store.select(selectCallId);
  public readonly call$: Observable<Call> = this.store.select(selectCall);
  public readonly loaded$: Observable<boolean> = this.store.select(selectLoaded).pipe(
    filter(loaded => Boolean(loaded))
  );
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly currentMediaStreamId$: Observable<string> = this.store.select(selectCurrentStreamId).pipe(
    filter(streamId => Boolean(streamId))
  );
  public readonly connectionData$: Observable<CallConnectionData> = this.store.select(selectConnectionData).pipe(
    filter(connectionData => Boolean(connectionData))
  );
  public readonly mediaTracks$: Observable<MediaStreamTrack[]> = this.store.select(selectMediaTracks).pipe(
    filter(connectionData => Boolean(connectionData))
  );
  public readonly testMessage$: Observable<string> = this.store.select(selectTestMessage);

  constructor(private store: Store<CallState>) { }

  public loadCall(callId: GUID): void {
    this.store.dispatch(callActions.setCallId({ callId }));
    this.store.dispatch(callActions.loadCall());
  }

  public setCurrentMediaStream(stream: Observable<MediaStream>): void {
    this.store.dispatch(callActions.setCurrentMediaStream({ stream }));
  }

  public setConnectionData(connectionData: CallConnectionData): void {
    this.store.dispatch(callActions.setConnectionData(connectionData));
  }

  public sendTestMessage(): void {
    this.store.dispatch(callActions.testSendMessage({ message: 'aaa' }));
  }

  public disconnectCallHub(): void {
    this.store.dispatch(stopSignalRHub(callHub));
  }
}
