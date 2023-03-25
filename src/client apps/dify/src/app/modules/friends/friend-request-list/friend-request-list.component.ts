import { Component } from '@angular/core';
import { FriendRequestType } from '@core/friends/friends.setting';
import { FriendRequestTile } from '@core/friends/friends.models';
import { guid } from '@shared/custom-types';

@Component({
  selector: 'app-friend-request-list',
  templateUrl: './friend-request-list.component.html',
  styleUrls: ['./friend-request-list.component.scss']
})
export class FriendRequestListComponent {
  public readonly requestTypes = FriendRequestType;
  public requestType: FriendRequestType = FriendRequestType.new;
  public readonly friendRequests: FriendRequestTile[] = [
    {
      id: guid(''),
      name: 'Ronald Waller',
      avatarUrl: 'https://i.gyazo.com/89238eb9fa6420a970c040f9c28f4d99.jpg'
    },
    {
      id: guid(''),
      name: 'Savanna Knight',
      avatarUrl: 'https://i.gyazo.com/8debadba827f5868f223ed2fa978c6d7.jpg'
    },
    {
      id: guid(''),
      name: 'Skylar Dudley',
      avatarUrl: 'https://i.gyazo.com/fc7a35c42de50e99044d6ea3598e377c.jpg'
    },
  ];
}
