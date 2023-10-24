import { AfterViewInit, Component, ElementRef, Input, ViewChild } from '@angular/core';
import { CallParticipantCard } from '@modules/call/models/call.models';

@Component({
  selector: 'app-call-participant',
  templateUrl: './call-participant.component.html',
  styleUrls: ['./call-participant.component.scss']
})
export class CallParticipantComponent implements AfterViewInit {
  @ViewChild('participantVideo') private _video: ElementRef;
  @Input() participantCard: CallParticipantCard;

  public ngAfterViewInit(): void {
    if(this.participantCard.stream) {
      this._video.nativeElement.srcObject = this.participantCard.stream;
    }
  }
}
