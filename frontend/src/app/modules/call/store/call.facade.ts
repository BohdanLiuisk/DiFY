import { Injectable } from '@angular/core';
import { 
  selectCall, 
  selectCallId, 
  selectCurrentMediaStream, 
  selectCurrentParticipantCard, 
  selectCurrentPeerId, 
  selectCurrentStream, 
  selectHubConnected, 
  selectLoaded, 
  selectLoading, 
  selectParticipantById,
  selectParticipantByStreamId, 
  selectParticipantCards, 
  selectParticipants, 
  selectParticipantsCount 
} from './call.selectors';
import { filterEmpty } from '@core/utils/pipe.operators';
import { Store } from '@ngrx/store';
import { GUID } from '@shared/custom-types';
import { Observable } from 'rxjs';
import { Call, Participant, CallParticipantCard, CallState } from '../models/call.models';
import { callActions } from './call.actions';

@Injectable()
export class CallFacade {
  public readonly callId$: Observable<GUID> = this.store.select(selectCallId).pipe(filterEmpty());
  public readonly call$: Observable<Call> = this.store.select(selectCall).pipe(filterEmpty());
  public readonly loaded$: Observable<boolean> = this.store.select(selectLoaded).pipe(filterEmpty());
  public readonly hubConnected$: Observable<boolean> = this.store.select(selectHubConnected).pipe(filterEmpty());
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly participants$: Observable<Participant[]> = this.store.select(selectParticipants).pipe(filterEmpty());
  public readonly participantCards$: Observable<CallParticipantCard[]> = this.store.select(selectParticipantCards).pipe(filterEmpty());
  public readonly participantCardsCount$: Observable<number> = this.store.select(selectParticipantsCount);
  public readonly currentMediaStream$: Observable<MediaStream> = this.store.select(selectCurrentMediaStream).pipe(filterEmpty());
  public readonly currentStream$: Observable<MediaStream> = this.store.select(selectCurrentStream).pipe(filterEmpty());
  public readonly currentPeerId$: Observable<string> = this.store.select(selectCurrentPeerId).pipe(filterEmpty());
  public readonly currentCard$: Observable<CallParticipantCard> = this.store.select(selectCurrentParticipantCard).pipe(filterEmpty());

  constructor(private store: Store<CallState>) { }

  public setLoading(): void {
    this.store.dispatch(callActions.setLoading());
  }

  public selectParticipantByStreamId(streamId: string): Observable<Participant> {
    return this.store.select(selectParticipantByStreamId(streamId)).pipe(filterEmpty());
  }

  public selectParticipantById(id: number): Observable<Participant> {
    return this.store.select(selectParticipantById(id)).pipe(filterEmpty());
  }

  public setCallId(callId: GUID): void {
    this.store.dispatch(callActions.setCallId({ callId }));
  }

  public setCurrentParticipantId(id: number) {
    this.store.dispatch(callActions.setCurrentParticipantId({ id }));
  }

  public stopVideoStream(): void {
    this.store.dispatch(callActions.stopVideoStream());
  }

  public enableVideoStream(): void {
    this.store.dispatch(callActions.enableVideoStream());
  }

  public destroyMediaStream(): void {
    this.store.dispatch(callActions.destroyMediaStream());
  }

  public startCallHub(): void {
    this.store.dispatch(callActions.startCallHub());
  }

  public setPeerId(peerId: string): void {
    this.store.dispatch(callActions.setPeerId({ peerId }));
  }

  public joinCall(): void {
    this.store.dispatch(callActions.joinCall());
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
