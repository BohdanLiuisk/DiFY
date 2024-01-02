import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { FindFriendsStore } from '@modules/friends/store/find-friends/find-friends.store';

@Component({
  selector: 'app-find-friends',
  templateUrl: './find-friends.component.html',
  styleUrl: './find-friends.component.scss',
  providers: [FindFriendsStore]
})
export class FindFriendsComponent extends BaseComponent implements OnInit {
  public searchValue: string = '';

  constructor(public readonly store: FindFriendsStore) {
    super();
  }

  public ngOnInit(): void {
    this.searchFriends();
  }

  public searchFriends(): void {
    this.store.findFriends(this.searchValue);
  }
}
