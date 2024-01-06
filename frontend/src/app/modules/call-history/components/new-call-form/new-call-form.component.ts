import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateNewCallConfig, ParticipantForCall } from '../../models/call-history.models';
import { Observable, Subject, distinctUntilChanged, switchMap } from 'rxjs';
import { CallHistoryService } from '../../services/call-history.service';
import { DialogRef } from '@angular/cdk/dialog';

@Component({
  selector: 'app-new-call-form',
  templateUrl: './new-call-form.component.html',
  styleUrl: './new-call-form.component.scss'
})
export class NewCallFormComponent {
  private readonly searchParticipant$: Subject<string> = new Subject<string>();

  public readonly participantsList$: Observable<ParticipantForCall[]> = this.searchParticipant$.pipe(
    distinctUntilChanged(),
    switchMap((search: string) => {
      return this.callHistoryService.searchParticipants(search);
    })
  );

  public readonly newCallForm: FormGroup = new FormGroup({
    callName: new FormControl('',  [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(100)
    ]),
    participants: new FormControl<ParticipantForCall[]>([], [
      Validators.required
    ])
  });
  
  constructor(
    private callHistoryService: CallHistoryService, 
    public dialogRef: DialogRef<CreateNewCallConfig>) { }

  public searchParticipants(search: string | null): void {
    this.searchParticipant$.next(search || '');
  }

  public submit(): void {
    if(this.newCallForm.valid) {
      const name: string = this.newCallForm.value.callName;
      const participants: ParticipantForCall[] = this.newCallForm.value.participants;
      this.dialogRef.close({ 
        name, 
        participantIds: participants.map(p => p.id)
      });
    } else {
      this.newCallForm.markAsDirty();
    }
  }

  public cancel(): void {
    this.dialogRef.close();
  }
}
