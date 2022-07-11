import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { NewUser } from '@core/models/auth/new-user.model';

@Injectable()
export class SignUpService {
  private readonly signUpPath: string = '/userAccess/userRegistrations';

  constructor(private httpService: CoreHttpService) { }

  public signUp(newUser: NewUser) {
    return this.httpService.postRequest(this.signUpPath, newUser);
  }
}
