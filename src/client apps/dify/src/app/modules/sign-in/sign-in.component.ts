import { Component, OnInit } from '@angular/core';
import { AuthService } from '@core/services/auth/auth.service';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { dify } from '@shared/constans/app-settings';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  public loginForm: FormGroup;

  constructor(private authService: AuthService) { }

  public ngOnInit(): void {
    this.setUpLoginForm();
  }

  public get username(): AbstractControl {
    return this.loginForm.get('username')!;
  }

  public get password(): AbstractControl {
    return this.loginForm.get('password')!;
  }

  public submit(): void {
    if(this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.signIn({ username, password })
        .subscribe(() => { });
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
