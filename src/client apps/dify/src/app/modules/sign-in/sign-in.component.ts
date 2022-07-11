import { Component, OnInit } from '@angular/core';
import { AuthService } from '@core/services/auth/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  public username: string;
  public password: string;

  constructor(private authService: AuthService) { }

  public ngOnInit(): void { }

  public submit(): void {
    this.authService.signIn({ username: this.username, password: this.password })
      .subscribe(() => { });
  }
}
