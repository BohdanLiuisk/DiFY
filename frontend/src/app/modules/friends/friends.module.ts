import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { FriendsRoutingModule } from './friends-routing.module';
import { 
  FriendsSectionsFiltrationComponent 
} from './components/friends-sections-filtration/friends-sections-filtration.component';
import { FriendTileComponent } from './components/friend-tile/friend-tile.component';
import { MayKnownPeopleComponent } from './components/may-known-people/may-known-people.component';
import { MyFriendsListComponent } from './components/my-friends-list/my-friends-list.component';
import { FriendRequestTileComponent } from './components/friend-request-tile/friend-request-tile.component';
import { FriendRequestListComponent } from './components/friend-request-list/friend-request-list.component';
import { FriendsComponent } from './components/friends/friends.component';
import { MyFriendsComponent } from './components/my-friends/my-friends.component';
import { FindFriendsComponent } from './components/find-friends/find-friends.component';
import { ButtonModule } from 'primeng/button';
import { BadgeModule } from 'primeng/badge';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { 
  FriendTileAvatarComponent 
} from './components/friend-tile-avatar/friend-tile-avatar.component';
import { TagModule } from 'primeng/tag';

@NgModule({
  declarations: [
    FriendsComponent,
    FriendsSectionsFiltrationComponent,
    FriendTileComponent,
    MayKnownPeopleComponent,
    MyFriendsListComponent,
    MyFriendsComponent,
    FriendRequestTileComponent,
    FriendRequestListComponent,
    FindFriendsComponent,
    FriendTileAvatarComponent
  ],
  imports: [
    SharedModule,
    FriendsRoutingModule,
    ButtonModule,
    BadgeModule,
    InputTextModule,
    OverlayPanelModule,
    TagModule
  ]
})
export class FriendsModule { }
