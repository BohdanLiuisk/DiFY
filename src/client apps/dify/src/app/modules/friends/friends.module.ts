import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { FriendsRoutingModule } from './friends-routing.module';
import { FriendsComponent } from './friends.component';
import { FriendsSectionsFiltrationComponent } from './friends-sections-filtration/friends-sections-filtration.component';
import { FriendTileComponent } from './friend-tile/friend-tile.component';
import { MayKnownPeopleComponent } from './may-known-people/may-known-people.component';
import { MyFriendsListComponent } from './my-friends-list/my-friends-list.component';
import { FriendRequestTileComponent } from './friend-request-tile/friend-request-tile.component';

@NgModule({
  declarations: [
    FriendsComponent,
    FriendsSectionsFiltrationComponent,
    FriendTileComponent,
    MayKnownPeopleComponent,
    MyFriendsListComponent,
    FriendRequestTileComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    FriendsRoutingModule
  ]
})
export class FriendsModule { }
