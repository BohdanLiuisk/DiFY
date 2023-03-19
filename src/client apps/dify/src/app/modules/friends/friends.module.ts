import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { FriendsRoutingModule } from './friends-routing.module';
import { FriendsComponent } from './friends.component';
import { FriendsSectionsFiltrationComponent } from './friends-sections-filtration/friends-sections-filtration.component';

@NgModule({
  declarations: [
    FriendsComponent,
    FriendsSectionsFiltrationComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    FriendsRoutingModule
  ]
})
export class FriendsModule { }
