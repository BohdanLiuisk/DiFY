import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { CallParticipantCard } from '@core/calls/store/call/call.models';

@Component({
  selector: 'app-call-participant',
  templateUrl: './call-participant.component.html',
  styleUrls: ['./call-participant.component.scss']
})
export class CallParticipantComponent implements OnInit, AfterViewInit {
  @ViewChild('participantVideo') private _participantVideo: ElementRef;
  @ViewChild('participantAudio') private _participantAudio: ElementRef;
  @Input() callParticipant: CallParticipantCard;

  constructor() { }

  public ngOnInit(): void {

  }

  public ngAfterViewInit(): void {
    if(this.callParticipant.stream) {
      this._participantVideo.nativeElement.srcObject = this.callParticipant.stream;
      //this._participantAudio.nativeElement.srcObject = this.callParticipant.stream;
    }
  }
}
