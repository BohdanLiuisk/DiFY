import { Component, EventEmitter, Output } from '@angular/core';
import { FriendCategory } from '@core/friends/friends.models';
import { FriendshipSections } from '@core/friends/friends.setting';

@Component({
  selector: 'app-friends-sections-filtration',
  templateUrl: './friends-sections-filtration.component.html',
  styleUrls: ['./friends-sections-filtration.component.scss']
})
export class FriendsSectionsFiltrationComponent {
  @Output('sectionFiltration') public sectionFiltration: EventEmitter<FriendshipSections> =
    new EventEmitter<FriendshipSections>();
  @Output('categoryFilter') public categoryFilter: EventEmitter<FriendCategory> =
    new EventEmitter<FriendCategory>();
  public readonly sections = FriendshipSections;
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
