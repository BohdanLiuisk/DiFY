import { Component } from '@angular/core';
import { FriendRequestType } from '@modules/friends/store/friends.setting';
import { FriendRequestTile } from '@modules/friends/store/friends.models';

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
      id: 2,
      name: 'Ronald Waller',
      avatarUrl: 'https://i.gyazo.com/89238eb9fa6420a970c040f9c28f4d99.jpg',
      online: true
    },
    {
      id: 2,
      name: 'Savanna Knight',
      avatarUrl: 'https://i.gyazo.com/8debadba827f5868f223ed2fa978c6d7.jpg',
      online: false
    },
    {
      id: 2,
      name: 'Skylar Dudley',
      avatarUrl: 'https://i.gyazo.com/fc7a35c42de50e99044d6ea3598e377c.jpg',
      online: true
    },
  ];
}
