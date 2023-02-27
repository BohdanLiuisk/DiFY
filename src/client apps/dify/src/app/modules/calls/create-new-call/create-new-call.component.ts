import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TuiDialogContext } from '@taiga-ui/core';
import { TUI_VALIDATION_ERRORS } from '@taiga-ui/kit';
import { POLYMORPHEUS_CONTEXT } from '@tinkoff/ng-polymorpheus';

@Component({
    selector: 'app-create-new-call',
    templateUrl: './create-new-call.component.html',
    styleUrls: ['./create-new-call.component.scss'],
    providers: [
      {
          provide: TUI_VALIDATION_ERRORS,
          useValue: {
              required: 'Value is required'
          }
      }
    ]
})
export class CreateNewCallComponent {
  public newCallForm: FormGroup = new FormGroup({
    callName: new FormControl('',  [Validators.required])
  });

  constructor(
    @Inject(POLYMORPHEUS_CONTEXT)
    private readonly context: TuiDialogContext<{ name: string }>) { }

  public submit(): void {
    if(this.newCallForm.valid) {
      const { callName } = this.newCallForm.value;
      this.context.completeWith({ name: callName });
    } else {
      this.newCallForm.markAllAsTouched();
    }
  }
}
