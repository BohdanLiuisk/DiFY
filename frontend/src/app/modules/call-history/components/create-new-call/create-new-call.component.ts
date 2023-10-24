import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateNewCallConfig, ParticipantForCall } from '@modules/call-history/models/call-history.models';
import { CallHistoryService } from '@modules/call-history/services/call-history.service';
import { CallHistoryFacade } from '@modules/call-history/store/call-history.facade';
import { TuiContextWithImplicit, TuiIdentityMatcher, TuiStringHandler } from '@taiga-ui/cdk';
import { TuiDialogContext } from '@taiga-ui/core';
import { BehaviorSubject, Observable, debounceTime, switchMap } from 'rxjs';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';

@Component({
  selector: 'app-create-new-call',
  templateUrl: './create-new-call.component.html',
  styleUrls: ['./create-new-call.component.scss']
})
export class CreateNewCallComponent implements OnInit {
  @Output('closeEvent') 
  public closeEvent: EventEmitter<void> = new EventEmitter<void>();
  public opened: boolean = true;
  public searchParticipant$: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public participantsList$: Observable<ParticipantForCall[]>;
  public newCallForm: FormGroup = new FormGroup({
    callName: new FormControl('',  [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(100)
    ]),
    participants: new FormControl<ParticipantForCall[]>([], [
      Validators.required
    ])
  });
  public readonly stringifyParticipants: TuiStringHandler<ParticipantForCall | TuiContextWithImplicit<ParticipantForCall>> = 
    item => 'name' in item ? item.name : item.$implicit.name;
  public readonly participantsMatcher: TuiIdentityMatcher<ParticipantForCall> = 
    (participant1, participant2) => participant1.id === participant2.id;
  public get nameField() {
    return this.newCallForm.get('callName');
  }
  public get participantsField() {
    return this.newCallForm.get('participants');
  }

  constructor(
    @Inject(POLYMORPHEUS_CONTEXT) private readonly context: TuiDialogContext<CreateNewCallConfig>,
    private callHistoryService: CallHistoryService, 
    public callHistoryFacade: CallHistoryFacade) {  }

  public ngOnInit(): void {
    this.participantsList$ = this.searchParticipant$.pipe(
      debounceTime(500),
      switchMap((search: string) => this.callHistoryService.searchParticipants(search))
    );
  }

  public submit(): void {
    if(this.newCallForm.valid) {
      const callName: string = this.newCallForm.value.callName;
      const participants: ParticipantForCall[] = this.newCallForm.value.participants;
      this.context.completeWith({ 
        name: callName, 
        participantIds: participants.map(p => p.id) 
      });
    } else {
      this.newCallForm.markAllAsTouched();
    }
  }

  public cancel(): void {
    this.context.completeWith(null);
  }

  public searchParticipants(search: string | null): void {
    this.searchParticipant$.next(search || '');
  }
}
