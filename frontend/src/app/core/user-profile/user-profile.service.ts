import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { Observable } from 'rxjs';
import { User } from '@core/models/user';

@Injectable({ providedIn: 'root' })
export class UserProfileService {
  private readonly profilePath: string = '/api/users/getById';

  constructor(private httpService: CoreHttpService) { }

  public getUserProfile(userId: number): Observable<User> {
    return this.httpService.getRequest<User>(`${this.profilePath}/${userId}`);
  }
}
