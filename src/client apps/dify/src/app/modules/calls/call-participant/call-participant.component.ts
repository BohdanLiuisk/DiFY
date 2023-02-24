import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { CallFacade } from '@core/calls/store/call/call.facade';
import { CallParticipantCard } from '@core/calls/store/call/call.models';
import { BaseComponent } from '@core/components/base.component';

@Component({
  selector: 'app-call-participant',
  templateUrl: './call-participant.component.html',
  styleUrls: ['./call-participant.component.scss']
})
export class CallParticipantComponent extends BaseComponent implements OnInit, AfterViewInit {
  @ViewChild('participantVideo') private _video: ElementRef;
  @Input() participantCard: CallParticipantCard;

  constructor(public readonly callFacade: CallFacade) {
    super();
  }

  public ngOnInit(): void {

  }

  public ngAfterViewInit(): void {
    if(this.participantCard.stream) {
      this._video.nativeElement.srcObject = this.participantCard.stream;
    }
  }

  public switchCamera(): void {
    if(this.participantCard.videoEnabled) {
      this.callFacade.stopVideoStream();
    } else {
      this.callFacade.enableVideoStream();
    }
  }
}
