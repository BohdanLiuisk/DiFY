import { Component } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { FriendshipSections, myFriendsFiltration } from '@core/friends/friends.setting';
import {FriendCategory, FriendRequestTile} from '@core/friends/friends.models';
import { guid } from '@shared/custom-types';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent extends BaseComponent {
  public currentSection: FriendshipSections = FriendshipSections.myFriends;
  public readonly sections = FriendshipSections;
  public readonly friendsSections = FriendshipSections;
  public myFriendsFilter = myFriendsFiltration.all;
  public categoryFilter: FriendCategory;

  public readonly friendRequestPreview: FriendRequestTile = {
    id: guid('9f655687-ed72-46d5-99de-1efcd9ed626d'),
    name: 'Coby Conrad',
    avatarUrl: 'https://i.gyazo.com/38d9a386346afc009c0fbd03d0e7e552.jpg',
    direction: ''
  };
}
