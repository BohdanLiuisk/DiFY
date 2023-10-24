import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { FriendsRoutingModule } from './friends-routing.module';
import { FriendsSectionsFiltrationComponent } from './components/friends-sections-filtration/friends-sections-filtration.component';
import { FriendTileComponent } from './components/friend-tile/friend-tile.component';
import { MayKnownPeopleComponent } from './components/may-known-people/may-known-people.component';
import { MyFriendsListComponent } from './components/my-friends-list/my-friends-list.component';
import { FriendRequestTileComponent } from './components/friend-request-tile/friend-request-tile.component';
import { FriendRequestListComponent } from './components/friend-request-list/friend-request-list.component';
import { FriendsComponent } from './components/friends/friends.component';

@NgModule({
  declarations: [
    FriendsComponent,
    FriendsSectionsFiltrationComponent,
    FriendTileComponent,
    MayKnownPeopleComponent,
    MyFriendsListComponent,
    FriendRequestTileComponent,
    FriendRequestListComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    FriendsRoutingModule
  ]
})
export class FriendsModule { }
