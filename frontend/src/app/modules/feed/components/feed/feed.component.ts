import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ParticipantForCall } from '@modules/call-history/models/call-history.models';
import { CallHistoryService } from '@modules/call-history/services/call-history.service';
import { Observable, Subject, distinctUntilChanged, switchMap } from 'rxjs';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent {
  public searchParticipant$: Subject<string> = new Subject<string>();
  public participantsList$: Observable<ParticipantForCall[]> = this.searchParticipant$.pipe(
    distinctUntilChanged(),
    switchMap((search: string) => {
      return this.callHistoryService.searchParticipants(search);
    })
  );
  public newCallForm: FormGroup = new FormGroup({
    participants: new FormControl<ParticipantForCall[]>([], [
      Validators.required
    ])
  });
  
  constructor(private callHistoryService: CallHistoryService) { }

  public searchParticipants(search: string | null): void {
    this.searchParticipant$.next(search || '');
  }

  public disableMultiselect(): void {
    this.newCallForm.disable();
  }

  public enableMultiselect(): void {
    this.newCallForm.enable();
  }
}
