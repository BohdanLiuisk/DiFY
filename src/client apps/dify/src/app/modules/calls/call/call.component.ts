import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CallFacade } from '@core/calls/store/call/call.facade';
import { environment } from '@env/environment';
import { GUID } from '@shared/custom-types';
import Peer from 'peerjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent implements OnInit {
  public peer: Peer;

  constructor(
    public readonly callFacade: CallFacade,
    private route: ActivatedRoute) { }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const callId: GUID = params['id'];
      this.callFacade.loadCall(callId);
      this.callFacade.loaded$.subscribe(() => {
        this.peer = new Peer(environment.peerOptions);
        this.peer.on('open', (id) => this._peerOpened(id));
      });
      this.callFacade.call$.subscribe((call) => {

      });
    });
  }

  private _peerOpened(id: string): void {

  }
}
