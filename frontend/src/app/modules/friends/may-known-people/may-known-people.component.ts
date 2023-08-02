import { Component, Input } from '@angular/core';
import { MayKnownMan } from '@core/friends/friends.models';

@Component({
  selector: 'app-may-known-people',
  templateUrl: './may-known-people.component.html',
  styleUrls: ['./may-known-people.component.scss']
})
export class MayKnownPeopleComponent {
  @Input('peoples') public peoples: MayKnownMan[] = [
    {
      id: '9f655687-ed72-46d5-99de-1efcd9ed626d',
      name: 'Kamilla Daychenko',
      avatarUrl: 'https://i.gyazo.com/6776b3bc60a0afc59db555bc71eaff31.jpg',
      online: true,
      mutualFriends: 5
    },
    {
      id: '9f655687-ed72-46d5-99de-1efcd9ed626d',
      name: 'Yuriy Chikovsky',
      avatarUrl: 'https://i.gyazo.com/9d8e3dcd7b23e3584d466a608975f3dd.jpg',
      online: false,
      mutualFriends: 3
    },
    {
      id: '9f655687-ed72-46d5-99de-1efcd9ed626d',
      name: 'Jayla Stevens',
      avatarUrl: 'https://i.gyazo.com/b0561b600fa955778b96280cef6903fa.jpg',
      online: true,
      mutualFriends: 1
    }
  ];
}
