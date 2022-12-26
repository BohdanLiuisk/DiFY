import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { CallSignalrEvents } from '@core/calls/store/call-signalr.events';
import { CallFacade } from '@core/calls/store/call/call.facade';
import { BaseComponent } from '@core/components/base.component';
import { environment } from '@env/environment';
import { GUID } from '@shared/custom-types';
import Peer from 'peerjs';
import { combineLatest, from, Observable, Subject, startWith, scan, skip } from 'rxjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('currentVideo') private currentVideo: ElementRef
  public peer: Peer;
  public currentStream: MediaStream;
  public connectedStreams: MediaStream[] = [];
  public connectedPeers: Map<string, MediaStream> = new Map<string, MediaStream>();
  public videoEnabled: Subject<void> = new Subject<void>();
  public videoEnabled$: Observable<boolean> = this.videoEnabled.asObservable().pipe(
    scan((state) => !state, true),
    startWith(true)
  );

  constructor(
    public readonly callFacade: CallFacade,
    private authFacade: AuthFacade,
    private signarEvents: CallSignalrEvents,
    private route: ActivatedRoute) {
    super();
  }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const callId: GUID = params['id'];
      this.callFacade.loadCall(callId);
      this._configurePeer();
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
          .then(videoStream => this._enableVideo(videoStream));
      });
    });
    this._subscribeSignalrEvents();
  }

  public ngAfterViewInit(): void {
    this._getMediaStream().pipe(this.untilThis).subscribe((currentStream => {
      this.currentStream = currentStream;
      this.callFacade.setCurrentMediaStreamId(currentStream.id);
      this.connectedStreams.push(currentStream);
      this.currentVideo.nativeElement.srcObject = currentStream;
    }));
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
    this.signarEvents.callParticipantConnected$.pipe(this.untilThis).subscribe((connection) => {
      console.log('participant joined', connection);
      this._callParticipant(connection.peerId);
    });
    this.signarEvents.participantLeft$.pipe(this.untilThis).subscribe((connection) => {
      console.log('participant left', connection);
    });
  }

  private _configurePeer(): void {
    this.callFacade.loaded$.pipe(this.untilThis).subscribe((loaded) => {
      console.log('loaded', loaded);
      this.peer = new Peer(environment.peerOptions);
      this.peer.on('open', (id) => this._peerOpened(id));
      this.peer.on('call', (call) => {
        call.on('stream', (stream) => {
          if (!this.connectedStreams.includes(stream)) {
            this.connectedStreams.push(stream);
          }
          this.connectedPeers.set(call.peer, stream);
        });
        call.answer(this.currentStream);
      });
    });
  }

  private _peerOpened(peerId: string): void {
    combineLatest([
        this.callFacade.callId$.pipe(this.untilThis),
        this.callFacade.currentMediaStreamId$.pipe(this.untilThis),
        this.authFacade.userId$.pipe(this.untilThis)])
      .pipe(this.untilThis)
      .subscribe(([callId, streamId, userId]) => {
        this.callFacade.setConnectionData(peerId, callId, streamId, userId);
        this.callFacade.invokeParticipantJoinedCall(peerId, callId, streamId, userId);
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
      }
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
