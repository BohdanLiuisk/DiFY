import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { BaseComponent } from '@core/components/base.component';
import { filterEmpty } from '@core/utils/pipe.operators';
import { environment } from '@env/environment';
import { CallParticipantCard } from '@modules/call/models/call.models';
import { CallSignalrEventsService } from '@modules/call/services/call-signalr-events.service';
import { CallFacade } from '@modules/call/store/call.facade';
import { GUID } from '@shared/custom-types';
import Peer from 'peerjs';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
  private _peer: Peer;
  public readonly participantCards$: Observable<CallParticipantCard[]> = this.callFacade.participantCards$
    .pipe(this.untilThis);
  public readonly currentCard$: Observable<CallParticipantCard> = this.callFacade.currentCard$
    .pipe(this.untilThis);
  public readonly loading$: Observable<boolean> = this.callFacade.loading$
    .pipe(this.untilThis);
  public readonly participantsCount$: Observable<number> = this.callFacade.participantCardsCount$
    .pipe(this.untilThis, filterEmpty());
  @ViewChild('participants') private _paricipantsDiv: ElementRef;

  constructor(
    public readonly callFacade: CallFacade,
    private signarEvents: CallSignalrEventsService,
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

  public switchCamera(currentCard: CallParticipantCard) {
    if(currentCard.videoEnabled) {
      this.callFacade.stopVideoStream();
    } else {
      this.callFacade.enableVideoStream();
    }
  }

  private _destroyPeer(): void {
    this._peer?.disconnect();
    this._peer?.destroy();
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
      const keys = Object.keys(this._peer.connections);
      keys.forEach(key => {
        const peerConnection = this._peer.connections[key];
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
    this._peer = new Peer(environment.peerOptions);
    this._peer.on('open', (id) => this._peerOpened(id));
    this._peer.on('call', (call) => {
      call.on('stream', (stream) => {
        this.callFacade.addParticipantCard(stream);
      });
      this.callFacade.currentMediaStream$.pipe(this.untilThis).subscribe((currentStream) => {
        call.answer(currentStream);
      });
    });
  }

  private _peerOpened(peerId: string): void {
    this.callFacade.setPeerId(peerId);
    this.callFacade.joinCall();
  }

  private _callParticipant(peerId: string): void {
    this.callFacade.currentMediaStream$.pipe(this.untilThis).subscribe((currentStream) => {
      const call = this._peer.call(peerId, currentStream);
      call?.on('stream', (stream) => {
        this.callFacade.addParticipantCard(stream);
      });
    });
  }
}
