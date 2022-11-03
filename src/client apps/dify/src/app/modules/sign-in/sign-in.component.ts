import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { dify } from '@shared/constans/app-settings';
import { AuthFacade } from '@core/auth/store/auth.facade';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  public hidePassword: boolean = true;
  public loginForm: FormGroup;

  constructor(private authFacade: AuthFacade) { }

  public ngOnInit(): void {
    this.setUpLoginForm();
  }

  public get username(): AbstractControl {
    return this.loginForm.get('username');
  }

  public get password(): AbstractControl {
    return this.loginForm.get('password');
  }

  public submit(): void {
    if(this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authFacade.login(username, password);
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  private setUpLoginForm(): void {
    const empty = dify.emptyString;
    this.loginForm = new FormGroup({
      'username': new FormControl(empty, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(15)
      ]),
      'password': new FormControl(empty, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(20)
      ]),
    });
  }
}
