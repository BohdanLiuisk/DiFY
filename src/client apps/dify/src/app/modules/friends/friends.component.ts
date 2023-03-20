import { Component } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { FriendshipSections, myFriendsFiltration } from '@core/friends/friends.setting';
import { FriendTile } from '@core/friends/friends.models';
import { guid } from '@shared/custom-types';

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

  public readonly friendTiles: FriendTile[] = [
    {
      id: guid('9f655687-ed72-46d5-99de-1efcd9ed626d'),
      name: 'Bogdan Liusik',
      avatarUrl: 'https://i.gyazo.com/a0dc4ac688811597f15f2e2387feee2d.png',
      online: true,
      workplace: 'Terrasoft',
      tags: [
        {
          name: 'Best friends',
          color: 'bg-teal-500'
        },
        {
          name: 'Family',
          color: 'bg-sky-400'
        },
        {
          name: 'School friend',
          color: 'bg-amber-400'
        }
      ]
    },
    {
      id: guid('9f655687-ed72-46d5-99de-1efcd9ed626d'),
      name: 'Mad Max',
      avatarUrl: 'https://daisyui.com/images/stock/photo-1534528741775-53994a69daeb.jpg',
      online: false,
      workplace: 'National Aviation University',
      tags: [
        {
          name: 'School friend',
          color: 'bg-amber-400'
        }
      ]
    }
  ];

  public onSearchParamsActiveZone(active: boolean): void {
    this.searchParamsOpened = active && this.searchParamsOpened;
  }
}
