import { AfterViewInit, Component, ElementRef, EventEmitter, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CallSignalrEvents } from '@core/calls/store/call-signalr.events';
import { CallFacade } from '@core/calls/store/call/call.facade';
import { CallParticipantCard, Participant } from '@core/calls/store/call/call.models';
import { BaseComponent } from '@core/components/base.component';
import { environment } from '@env/environment';
import { GUID } from '@shared/custom-types';
import { TuiAlertService } from '@taiga-ui/core';
import Peer from 'peerjs';
import { combineLatest, from, Observable, Subject, startWith, scan, skip, mergeMap, map, withLatestFrom, filter } from 'rxjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('currentVideo') private currentVideo: ElementRef;
  public peer: Peer;
  public currentStream: MediaStream;
  public connectedStreams: MediaStream[] = [];
  public connectedPeers: Map<string, MediaStream> = new Map<string, MediaStream>();
  public participantCards: CallParticipantCard[] = [];
  public participantCards$: Observable<CallParticipantCard[]> = this.callFacade.participantCards$.pipe(this.untilThis);
  private currentStreamLoaded: EventEmitter<void> = new EventEmitter<void>();
  public videoEnabled: Subject<void> = new Subject<void>();
  public videoEnabled$: Observable<boolean> = this.videoEnabled.asObservable().pipe(
    scan((state) => !state, true),
    startWith(true)
  );

  constructor(
    public readonly callFacade: CallFacade,
    private signarEvents: CallSignalrEvents,
    private route: ActivatedRoute,
    @Inject(TuiAlertService)
    private readonly alertService: TuiAlertService) {
    super();
  }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const callId: GUID = params['id'];
      this.callFacade.setCallId(callId);
      this.callFacade.setLoading();
      this.callFacade.startCallHub();
      this.callFacade.hubConnected$.pipe(this.untilThis).subscribe(() => {
        this._getMediaStream().pipe(this.untilThis).subscribe((currentStream => {
          this.currentStream = currentStream;
          this.connectedStreams.push(currentStream);
          this.currentStreamLoaded.next();
          this.callFacade.setCurrentMediaStreamId(currentStream.id);
        }));
        this._configurePeer();
      });
      this.callFacade.loaded$.pipe(this.untilThis).subscribe((loaded) => {
        console.log('joined', loaded);
      });
      this.videoEnabled$.pipe(this.untilThis, skip(1)).subscribe((enabled) => {
        const videoTracks = this.currentStream.getVideoTracks();
        if (!enabled) {
          videoTracks.forEach(track => {
            track.enabled = false;
            track.stop();
          });
          return;
        }
        videoTracks.forEach((track) => {
          track.enabled = enabled;
        });
        navigator.mediaDevices.getUserMedia({ video: true })
          .then(videoStream =>
            this._enableVideo(videoStream));
      });
      this._subscribeSignalrEvents();
    });
  }

  public ngAfterViewInit(): void {
    this.currentStreamLoaded.pipe(this.untilThis).subscribe(() => {
      this.currentVideo.nativeElement.srcObject = this.currentStream;
    });
  }

  public override ngOnDestroy(): void {
    this.callFacade.clearState();
    this._destroyPeer();
    this.currentStream?.getTracks().forEach((track) => track.stop());
    this.callFacade.leftCallHub();
    super.ngOnDestroy();
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
  }

  private _configurePeer(): void {
    this.peer = new Peer(environment.peerOptions);
    this.peer.on('open', (id) => this._peerOpened(id));
    this.peer.on('call', (call) => {
      call.on('stream', (stream) => {
        if (!this.connectedStreams.includes(stream)) {
          this.connectedStreams.push(stream);
        }
        this.connectedPeers.set(call.peer, stream);
        this.callFacade.addParticipantCard(stream);
        this.callFacade.selectParticipantByStreamId(stream.id).pipe(this.untilThis)
          .subscribe((participant) => {
            this._createParticipantCard(participant);
          });
      });
      call.answer(this.currentStream);
    });
  }

  private _peerOpened(peerId: string): void {
    combineLatest([
      this.callFacade.callId$.pipe(this.untilThis),
      this.callFacade.currentMediaStreamId$.pipe(this.untilThis)])
    .pipe(this.untilThis)
    .subscribe(([callId, streamId]) => {
      this.callFacade.setJoinData(streamId, peerId, callId);
      this.callFacade.joinCall();
    });
  }

  private _callParticipant(peerId: string): void {
    const call = this.peer.call(peerId, this.currentStream);
    call?.on('stream', (stream) => {
      if (this.connectedStreams.includes(stream)) return;
      this.connectedStreams.push(stream);
      const connectedPeer = this.connectedPeers.get(call.peer);
      if (!connectedPeer || connectedPeer.id !== stream.id) {
        this.connectedPeers.set(call.peer, stream);
        this.callFacade.addParticipantCard(stream);
        this.callFacade.selectParticipantByStreamId(stream.id).pipe(this.untilThis).subscribe((participant) => {
          this._createParticipantCard(participant);
        });
      }
    });
  }

  private _createParticipantCard(participant: Participant): void {
    const stream = this.connectedStreams.find((stream) => stream.id === participant.streamId);
    this.participantCards.push({
      participantId: participant.id,
      stream
    });
  }

  private _getMediaStream(): Observable<MediaStream> {
    return from(navigator.mediaDevices.getUserMedia({
      video: true,
      audio: false
    }));
  }

  private _enableVideo(mediaStream: MediaStream): void {
    this.currentVideo.nativeElement.srcObject = mediaStream;
    this.currentStream.getVideoTracks().forEach((videoTrack) => {
      this.currentStream.removeTrack(videoTrack);
    });
    const videoTrack = mediaStream.getVideoTracks()[0];
    this.currentStream.addTrack(videoTrack);
  }

  public logConnectedStreams() {
    console.info('ConnectedStreams');
    console.info(this.connectedStreams);
  }

  public logPeerConnections() {
    console.info('PeerConnections');
    console.info(this.peer.connections);
  }

  public logCurrentStream() {
    console.info('CurrentStream');
    console.info(this.currentStream);
  }
}
