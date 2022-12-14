import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CallState } from '@core/calls/store/call/call.store';
import { environment } from '@env/environment';
import { ComponentStore } from "@ngrx/component-store";
import Peer from 'peerjs';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss'],
  providers: [ComponentStore],
})
export class CallComponent implements OnInit {
  public peer: Peer;

  constructor(
    private readonly callStore: ComponentStore<CallState>,
    private route: ActivatedRoute)
    { }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log(params['id']);
    });
    this.peer = new Peer(environment.peerOptions);
    this.peer.on('open', (id) => this._peerOpened(id));
  }

  private _peerOpened(id: string): void {
    this.route.params.subscribe(params => {
      console.log(params['id']);
    });
  }
}
