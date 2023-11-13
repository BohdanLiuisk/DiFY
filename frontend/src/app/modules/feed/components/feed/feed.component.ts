import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ParticipantForCall } from '@modules/call-history/models/call-history.models';
import { CallHistoryService } from '@modules/call-history/services/call-history.service';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss'],
  providers: [CallHistoryService]
})
export class FeedComponent implements OnInit{
  public participantsList$: Observable<ParticipantForCall[]>;

  public searchParticipant$: BehaviorSubject<string> = new BehaviorSubject<string>('');

  public newCallForm: FormGroup = new FormGroup({
    participants: new FormControl<ParticipantForCall[]>([], [
      Validators.required
    ])
  });

  constructor(private callHistoryService: CallHistoryService) { }

  public ngOnInit(): void {
    this.participantsList$ = this.searchParticipant$.pipe(
      switchMap((search: string) => this.callHistoryService.searchParticipants(search))
    );
  }

  public showParticipants(): void {
    console.log(this.newCallForm.get('participants').value);
  }

  public enableDisable(): void {
    if(this.newCallForm.get('participants').enabled) {
      this.newCallForm.get('participants')?.disable();
    } else {
      this.newCallForm.get('participants')?.enable();
    }
  }

  public searchParticipants(search: string | null): void {
    this.searchParticipant$.next(search || '');
  }
}
