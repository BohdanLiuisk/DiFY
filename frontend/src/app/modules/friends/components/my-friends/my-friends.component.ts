import { Component } from '@angular/core';
import { guid } from '@shared/custom-types';
import { myFriendsFiltration } from '@modules/friends/models/friends.setting';
import { FriendCategory, FriendRequestTile } from '@modules/friends/models/friends.models';

@Component({
  selector: 'app-my-friends',
  templateUrl: './my-friends.component.html',
  styleUrl: './my-friends.component.scss'
})
export class MyFriendsComponent {
  public myFriendsFilter = myFriendsFiltration.all;
  public categoryFilter: FriendCategory;

  public readonly friendRequestPreview: FriendRequestTile = {
    id: 2,
    name: 'Coby Conrad',
    avatarUrl: 'https://i.gyazo.com/38d9a386346afc009c0fbd03d0e7e552.jpg',
    online: true
  };
}
