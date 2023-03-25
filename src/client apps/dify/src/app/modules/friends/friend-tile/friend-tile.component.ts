import { Component, Input } from '@angular/core';
import { FriendTile } from '@core/friends/friends.models';
@Component({
  selector: 'app-friend-tile',
  templateUrl: './friend-tile.component.html',
  styleUrls: ['./friend-tile.component.scss']
})
export class FriendTileComponent {
  @Input('friendTile') friendTile: FriendTile;
}