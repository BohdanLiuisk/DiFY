import {AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { CallSignalrEvents } from '@core/calls/store/call-signalr.events';
import { CallFacade } from '@core/calls/store/call/call.facade';
import { CallParticipantCard } from '@core/calls/store/call/call.models';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';
import { environment } from '@env/environment';
import { GUID } from '@shared/custom-types';
import Peer from 'peerjs';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
  public peer: Peer;
  public participantCards$: Observable<CallParticipantCard[]> = this.callFacade.participantCards$.pipe(this.untilThis);
  public participantsCount$: Observable<number> = this.callFacade.participantCardsCount$.pipe(this.untilThis, filterEmpty());
  @ViewChild('participants') private _paricipantsDiv: ElementRef;

  constructor(
    public readonly callFacade: CallFacade,
    private signarEvents: CallSignalrEvents,
    private route: ActivatedRoute,
    private authFacade: AuthFacade) {
    super();
  }

  public ngOnInit(): void {
    this.route.params.pipe(this.untilThis).subscribe(params => {
      const callId: GUID = params['id'];
      this.authFacade.userId$.pipe(this.untilThis).subscribe(id => {
        this.callFacade.setCurrentParticipantId(id);
      });
      this.callFacade.setCallId(callId);
      this.callFacade.setLoading();
      this.callFacade.startCallHub();
      this.callFacade.hubConnected$.pipe(this.untilThis).subscribe(() => {
        this._configurePeer();
      });
      this.callFacade.loaded$.pipe(this.untilThis).subscribe((loaded) => {
        console.log('joined', loaded);
      });
      this._subscribeSignalrEvents();
    });
  }

  public ngAfterViewInit(): void {
    this.participantsCount$.subscribe(count => {
      this._paricipantsDiv.nativeElement.style.gridTemplateColumns = `repeat(${count}, 1fr)`;
    });
  }

  public override ngOnDestroy(): void {
    super.ngOnDestroy();
    this.callFacade.destroyMediaStream();
    this.callFacade.clearState();
    this._destroyPeer();
    this.callFacade.leftCallHub();
  }

  private _destroyPeer(): void {
    this.peer?.disconnect();
    this.peer?.destroy();
  }

  private _subscribeSignalrEvents(): void {
    this.signarEvents.callParticipantConnected$.pipe(this.untilThis).subscribe((participant) => {
      console.log('participant joined', participant);
      this._callParticipant(participant.peerId);
    });
    this.signarEvents.participantLeft$.pipe(this.untilThis).subscribe((connection) => {
      console.log('participant left', connection);
    });
    this.signarEvents.updateVideoTrack$.pipe(this.untilThis).subscribe(({ videoTrack }) => {
      console.log('video track updated', videoTrack);
      const keys = Object.keys(this.peer.connections);
      keys.forEach(key => {
        const peerConnection = this.peer.connections[key];
        peerConnection?.forEach((pc) => {
          const sender = pc.peerConnection.getSenders().find((s) => {
            return s.track.kind === videoTrack.kind;
          });
          sender.replaceTrack(videoTrack);
        });
      });
    });
  }

  private _configurePeer(): void {
    this.peer = new Peer(environment.peerOptions);
    this.peer.on('open', (id) => this._peerOpened(id));
    this.peer.on('call', (call) => {
      call.on('stream', (stream) => {
        this.callFacade.addParticipantCard(stream);
      });
      this.callFacade.currentMediaStream$.pipe(this.untilThis).subscribe((currentStream) => {
        call.answer(currentStream);
      });
    });
  }

  private _peerOpened(peerId: string): void {
    this.callFacade.joinCall(peerId);
  }

  private _callParticipant(peerId: string): void {
    this.callFacade.currentMediaStream$.pipe(this.untilThis).subscribe((currentStream) => {
      const call = this.peer.call(peerId, currentStream);
      call?.on('stream', (stream) => {
        this.callFacade.addParticipantCard(stream);
      });
    });
  }
}
