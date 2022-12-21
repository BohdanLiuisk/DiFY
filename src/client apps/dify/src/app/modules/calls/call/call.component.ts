import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { CallFacade } from '@core/calls/store/call/call.facade';
import { BaseComponent } from '@core/components/base.component';
import { environment } from '@env/environment';
import { GUID } from '@shared/custom-types';
import Peer from 'peerjs';
import { combineLatest, from, Observable, BehaviorSubject, Subject, startWith, map, scan } from 'rxjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent extends BaseComponent implements OnInit, OnDestroy {
  public peer: Peer;
  public mediaEnabled: Subject<void> = new Subject<void>();
  public mediaEnabled$: Observable<boolean> = this.mediaEnabled.asObservable().pipe(
    scan((state) => !state, true),
    startWith(true)
  );

  constructor(
    public readonly callFacade: CallFacade,
    private authFacade: AuthFacade,
    private route: ActivatedRoute) {
    super();
  }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const callId: GUID = params['id'];
      this.callFacade.loadCall(callId);
      this.callFacade.connectionData$.subscribe((data) => {
        console.log('connectionData', data);
      });
      this.callFacade.currentMediaStreamId$.subscribe((data) => {
        console.log('currentMediaStreamId', data);
      });
      this.callFacade.callId$.subscribe((callId) => {
        console.log('callId', callId);
      });
      this.callFacade.call$.subscribe((call) => {
        console.log('call', call);
      });
      this.authFacade.userId$.subscribe((userId) => {
        console.log('userId', userId);
      });
      this.callFacade.loaded$.subscribe(() => {
        this.callFacade.setCurrentMediaStream(this._getMediaStream());
        this.peer = new Peer(environment.peerOptions);
        this.peer.on('open', (id) => this._peerOpened(id));
      });
      combineLatest([this.mediaEnabled$, this.callFacade.mediaTracks$])
        .pipe(this.untilThis)
        .subscribe(([mediaEnabled, mediaTracks]) => {
          if(!mediaEnabled) {
            mediaTracks.forEach(media => media.stop());
          } else {
            //this.callFacade.setCurrentMediaStream(this._getMediaStream());
          }
        });
    });
  }

  public override ngOnDestroy(): void {
    //this.callFacade.disconnectCallHub();
  }

  private _peerOpened(peerId: string): void {
    combineLatest([this.callFacade.callId$, this.callFacade.currentMediaStreamId$, this.authFacade.userId$])
      .pipe(this.untilThis)
      .subscribe(([callId, streamId, userId]) => {
        this.callFacade.setConnectionData({ peerId, callId, streamId, userId });
      });
  }

  private _getMediaStream(): Observable<MediaStream> {
    return from(navigator.mediaDevices.getUserMedia({
      video: true,
      audio: false
    }));
  }
}
