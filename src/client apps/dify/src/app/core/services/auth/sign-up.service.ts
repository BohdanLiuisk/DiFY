import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { NewUser } from '@core/models/auth/new-user.model';
import { NewUserRegistered } from '@core/models/user/new-user-registered.model';
import { filter, tap } from 'rxjs';
import { AuthService } from '@core/services/auth/auth.service';

@Injectable()
export class SignUpService {
  private readonly signUpPath: string = '/userAccess/userRegistrations';

  constructor(private httpService: CoreHttpService, private authService: AuthService) { }

  public signUp(newUser: NewUser) {
    return this.httpService.postRequest<NewUserRegistered>(this.signUpPath, newUser)
      .pipe(
        filter(registered => {
          return Boolean(registered.newUserId);
        }),
        tap(registered => {
          this.httpService.patchRequest(`${this.signUpPath}/${registered.newUserId}/confirm`)
            .subscribe(() => {
              this.authService.signIn({ username: newUser.login, password: newUser.password })
                .subscribe(() => { });
            });
        })
      );
  }
}
