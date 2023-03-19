import { Component } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { FriendshipSections, myFriendsFiltration } from '@core/friends/friends.setting';
@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent extends BaseComponent {
  public currentSection: FriendshipSections = FriendshipSections.all;
  public myFriendsFilter = myFriendsFiltration.all;
  public readonly sections = FriendshipSections;
  public friendsSearch: string = ``;
  public searchParamsOpened: boolean = false;

  public onSearchParamsActiveZone(active: boolean): void {
    this.searchParamsOpened = active && this.searchParamsOpened;
  }
}
