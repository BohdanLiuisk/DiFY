import { Injectable } from '@angular/core';
import { User } from '@core/models/user';
import { ReplaySubject } from 'rxjs';

@Injectable()
export class AuthEventsService {
  public readonly succesfullyAuthenticated = new ReplaySubject<User>(1);
  public readonly succesfullyAuthenticated$ = this.succesfullyAuthenticated.asObservable();
}
