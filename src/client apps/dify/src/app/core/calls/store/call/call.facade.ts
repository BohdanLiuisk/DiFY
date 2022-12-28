import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Observable } from "rxjs";
import { callActions } from '@core/calls/store/call/call.actions';
import {
  selectCall,
  selectCallId,
  selectCurrentMediaStreamId,
  selectHubConnected,
  selectJoinData,
  selectLoaded,
  selectLoading,
  selectParticipantById,
  selectParticipantByStreamId,
  selectParticipantCards,
  selectParticipants
} from "@core/calls/store/call/call.selectors";
import { Call, CallParticipantCard, CallState, JoinData, Participant } from "@core/calls/store/call/call.models";
import { filterEmpty } from "@core/utils/pipe.operators";

@Injectable({ providedIn: 'root' })
export class CallFacade {
  public readonly callId$: Observable<GUID> = this.store.select(selectCallId).pipe(filterEmpty());
  public readonly call$: Observable<Call> = this.store.select(selectCall).pipe(filterEmpty());
  public readonly loaded$: Observable<boolean> = this.store.select(selectLoaded).pipe(filterEmpty());
  public readonly hubConnected$: Observable<boolean> = this.store.select(selectHubConnected).pipe(filterEmpty());
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly currentMediaStreamId$: Observable<string> = this.store.select(selectCurrentMediaStreamId).pipe(filterEmpty());
  public readonly joinData$: Observable<JoinData> = this.store.select(selectJoinData).pipe(filterEmpty());
  public readonly participants$: Observable<Participant[]> = this.store.select(selectParticipants).pipe(filterEmpty());
  public readonly participantCards$: Observable<CallParticipantCard[]> = this.store.select(selectParticipantCards).pipe(filterEmpty());

  constructor(private store: Store<CallState>) { }

  public setLoading(): void {
    this.store.dispatch(callActions.setLoading());
  }

  public selectParticipantByStreamId(streamId: string): Observable<Participant> {
    return this.store.select(selectParticipantByStreamId(streamId)).pipe(filterEmpty());
  }

  public selectParticipantById(id: GUID): Observable<Participant> {
    return this.store.select(selectParticipantById(id)).pipe(filterEmpty());
  }

  public setCallId(callId: GUID): void {
    this.store.dispatch(callActions.setCallId({ callId }));
  }

  public startCallHub(): void {
    this.store.dispatch(callActions.startCallHub());
  }

  public joinCall(): void {
    this.store.dispatch(callActions.joinCall());
  }

  public setCurrentMediaStreamId(streamId: string): void {
    this.store.dispatch(callActions.setCurrentMediaStreamId({ streamId }));
  }

  public setJoinData(streamId: string, peerId: string, callId: GUID): void {
    this.store.dispatch(callActions.setJoinData({ streamId, peerId, callId }));
  }

  public addParticipantCard(stream: MediaStream): void {
    this.store.dispatch(callActions.addParticipantCard({ stream }));
  }

  public clearState(): void {
    this.store.dispatch(callActions.clearState());
  }

  public leftCallHub(): void {
    this.store.dispatch(callActions.stopCallHub());
  }
}
