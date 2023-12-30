import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FriendsComponent } from './components/friends/friends.component';
import { FriendRequestListComponent } from './components/friend-request-list/friend-request-list.component';
import { FindFriendsComponent } from './components/find-friends/find-friends.component';
import { MyFriendsComponent } from './components/my-friends/my-friends.component';

const routes: Routes = [
  {
    path: '',
    component: FriendsComponent,
    children: [
      {
        path: '',
        component: MyFriendsComponent
      },
      {
        path: 'requests',
        component: FriendRequestListComponent
      },
      {
        path: 'find',
        component: FindFriendsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FriendsRoutingModule { }
