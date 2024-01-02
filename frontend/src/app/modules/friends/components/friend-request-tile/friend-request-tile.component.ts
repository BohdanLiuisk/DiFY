import { Component, Input } from '@angular/core';
import { FriendRequestTile } from '@modules/friends/models/friends.models';

@Component({
  selector: 'app-friend-request-tile',
  templateUrl: './friend-request-tile.component.html',
  styleUrls: ['./friend-request-tile.component.scss']
})
export class FriendRequestTileComponent {
  @Input('friendRequestTile') friendRequestTile: FriendRequestTile;
}
