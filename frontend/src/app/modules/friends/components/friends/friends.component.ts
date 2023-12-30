import { Component } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { FriendshipSections, myFriendsFiltration } from '@modules/friends/store/friends.setting';
import { FriendCategory, FriendRequestTile } from '@modules/friends/store/friends.models';
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
  public categoryFilter: FriendCategory;
}
