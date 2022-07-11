import { Component, OnInit } from '@angular/core';
import { SignUpService } from '@core/services/auth/sign-up.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  public username: string;
  public password: string;
  public email: string;
  public firstName: string;
  public lastName: string;

  constructor(private signUpService: SignUpService) { }

  ngOnInit(): void { }

  submit(): void {
    this.signUpService.signUp({
      login: this.username,
      password: this.password,
      email: this.email,
      firstName: this.firstName,
      lastName: this.lastName
    }).subscribe(() => { });
  }
}
