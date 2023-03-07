import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { GUID } from '@shared/custom-types';
import { Observable, of } from 'rxjs';
import { UserInfo } from './store/user-profile.models';

@Injectable({ providedIn: 'root' })
export class UserProfileService {
  private readonly profilePath: string = '/api/social/user_profile';

  constructor(private httpService: CoreHttpService) { }

  public getUserProfile(userId: GUID): Observable<UserInfo> {
    return this.httpService.getRequest<UserInfo>(`${this.profilePath}/${userId}`);
  }
}
