import { Component, OnInit } from '@angular/core';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { dify } from '@shared/constans/app-settings';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  public signUpForm: FormGroup;

  constructor(private authFacade: AuthFacade) { }

  public ngOnInit(): void {
    this.setUpSignUpForm();
  }

  public submit(): void {
    if(this.signUpForm.valid) {
      const { login, password, email, firstName, lastName } = this.signUpForm.value;
      this.authFacade.signUp({ login, password, email, firstName,  lastName });
    } else {
      this.signUpForm.markAllAsTouched();
    }
  }

  public get username(): AbstractControl {
    return this.signUpForm.get('login');
  }

  public get password(): AbstractControl {
    return this.signUpForm.get('password');
  }

  public get email(): AbstractControl {
    return this.signUpForm.get('email');
  }

  public get firstName(): AbstractControl {
    return this.signUpForm.get('firstName');
  }

  public get lastName(): AbstractControl {
    return this.signUpForm.get('lastName');
  }

  private setUpSignUpForm(): void {
    const empty = dify.emptyString;
    this.signUpForm = new FormGroup({
      'login': new FormControl(empty, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(15)
      ]),
      'password': new FormControl(empty, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(20)
      ]),
      'email': new FormControl(empty, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(30)
      ]),
      'firstName': new FormControl(empty, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(30)
      ]),
      'lastName': new FormControl(empty, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(30)
      ])
    });
  }
}
