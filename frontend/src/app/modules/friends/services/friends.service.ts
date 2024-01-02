import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { FoundFriendsResponse } from '../models/friends.models';
import { Observable } from 'rxjs';

@Injectable()
export class FriendsService {
  private readonly friendsPath: string = '/api/friends';

  constructor(private httpService: CoreHttpService) { }

  public findFriends(page: number, perPage: number, searchValue: string): Observable<FoundFriendsResponse> {
    return this.httpService.getRequest<FoundFriendsResponse>(
      `${this.friendsPath}/findFriends?pageNumber=${page}&pageSize=${perPage}&searchValue=${searchValue}`);
  }
}
