import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { dify } from '@shared/constans/app-settings';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { TUI_VALIDATION_ERRORS } from '@taiga-ui/kit';
import { maxLengthValidator, minLengthValidator } from '@core/utils/validators';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
  providers: [
    {
      provide: TUI_VALIDATION_ERRORS,
      useValue: {
          required: 'Value is required',
          maxlength: maxLengthValidator,
          minlength: minLengthValidator
      }
    }
  ]
})
export class SignInComponent {
  public hidePassword: boolean = true;
  public readonly loginForm: FormGroup = new FormGroup({
    'username': new FormControl(dify.emptyString, [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(15)
    ]),
    'password': new FormControl(dify.emptyString, [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(20)
    ]),
  });

  constructor(private authFacade: AuthFacade) { }

  public submit(): void {
    if(this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authFacade.login(username, password);
    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}
