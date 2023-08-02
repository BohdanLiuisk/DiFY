import { Component, Input } from '@angular/core';
import { FriendTile } from '@core/friends/friends.models';
import { guid } from '@shared/custom-types';

@Component({
  selector: 'app-my-friends-list',
  templateUrl: './my-friends-list.component.html',
  styleUrls: ['./my-friends-list.component.scss']
})
export class MyFriendsListComponent {
  public friendsSearch: string = ``;
  public searchParamsOpened: boolean = false;
  @Input('friendTiles') public friendTiles: FriendTile[] = [
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