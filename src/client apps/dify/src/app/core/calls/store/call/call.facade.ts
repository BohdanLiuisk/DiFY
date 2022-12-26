import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Observable } from "rxjs";
import { callActions } from '@core/calls/store/call/call.actions';
import {
  selectCall,
  selectCallId,
  selectConnectionData,
  selectCurrentMediaStreamId,
  selectLoaded,
  selectLoading
} from "@core/calls/store/call/call.selectors";
import { Call, CallConnectionData, CallState } from "@core/calls/store/call/call.models";
import { filterEmpty } from "@core/utils/pipe.operators";

@Injectable({ providedIn: 'root' })
export class CallFacade {
  public readonly callId$: Observable<GUID> = this.store.select(selectCallId).pipe(filterEmpty());
  public readonly call$: Observable<Call> = this.store.select(selectCall);
  public readonly loaded$: Observable<boolean> = this.store.select(selectLoaded).pipe(filterEmpty());
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly currentMediaStreamId$: Observable<string> = this.store.select(selectCurrentMediaStreamId).pipe(filterEmpty());
  public readonly connectionData$: Observable<CallConnectionData> = this.store.select(selectConnectionData).pipe(filterEmpty());

  constructor(private store: Store<CallState>) { }

  public loadCall(callId: GUID): void {
    this.store.dispatch(callActions.setCallId({ callId }));
    this.store.dispatch(callActions.loadCall());
  }

  public setCurrentMediaStreamId(streamId: string): void {
    this.store.dispatch(callActions.setCurrentMediaStreamId({ streamId }));
  }

  public setConnectionData(peerId: string, callId: GUID, streamId: string, userId: GUID): void {
    this.store.dispatch(callActions.setConnectionData({ peerId, callId, streamId, userId }));
  }

  public invokeParticipantJoinedCall(peerId: string, callId: GUID, streamId: string, userId: GUID): void {
    this.store.dispatch(callActions.invokeParticipantJoined({ peerId, callId, streamId, userId }));
  }

  public clearState(): void {
    this.store.dispatch(callActions.clearState());
  }

  public leftCallHub(): void {
    this.store.dispatch(callActions.stopCallHub());
  }
}
