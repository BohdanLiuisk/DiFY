import { Component } from '@angular/core';
import { FriendCategory } from '@core/friends/friends.models';

@Component({
  selector: 'app-friends-sections-filtration',
  templateUrl: './friends-sections-filtration.component.html',
  styleUrls: ['./friends-sections-filtration.component.scss']
})
export class FriendsSectionsFiltrationComponent {
  public categoryDropdownOpened: boolean = false;

  public friendCategories: FriendCategory[] = [
    {
      id: 1, name: 'Family'
    },
    {
      id: 2, name: 'Best friends'
    },
    {
      id: 3, name: 'Colleagues'
    },
    {
      id: 4, name: 'University friends'
    },
    {
      id: 5, name: 'School friends'
    },
  ];


}
