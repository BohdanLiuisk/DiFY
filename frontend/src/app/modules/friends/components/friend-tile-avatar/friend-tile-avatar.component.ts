import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-friend-tile-avatar',
  templateUrl: './friend-tile-avatar.component.html',
  styleUrl: './friend-tile-avatar.component.scss'
})
export class FriendTileAvatarComponent {
  @Input('name') name: string;
  @Input('avatarUrl') avatarUrl: string;
  @Input('online') online: boolean;
}
