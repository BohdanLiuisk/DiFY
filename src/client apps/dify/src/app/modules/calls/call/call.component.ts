import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { callInitialState, CallStore } from '@core/calls/store/call/call.store';
import { environment } from '@env/environment';
import { provideComponentStore } from '@ngrx/component-store';
import { GUID } from '@shared/custom-types';
import Peer from 'peerjs';



@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss'],
  providers: [
    provideComponentStore(CallStore)
  ]
})
export class CallComponent implements OnInit {
  public peer: Peer;

  constructor(
    private readonly callStore: CallStore,
    private route: ActivatedRoute) { }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const callId: GUID = params['id'];
      this.callStore.setState(({...callInitialState, call: { ...callInitialState.call, id: callId }}));
      this.callStore.loaded$.subscribe(() => {
        this.peer = new Peer(environment.peerOptions);
        this.peer.on('open', (id) => this._peerOpened(id));
      });
    });
  }

  private _peerOpened(id: string): void {

  }
}
