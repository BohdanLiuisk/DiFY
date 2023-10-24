import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
const routes: Routes =  [
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: 'feed',
        loadChildren: async () => (await import('../feed/feed.module')).FeedModule
      },
      {
        path: 'call-history',
        loadChildren: async () => (await import('../call-history/call-history.module')).CallHistoryModule
      },
      {
        path: 'call',
        loadChildren: async () => (await import('../call/call.module')).CallModule
      },
      {
        path: 'profile',
        loadChildren: async () => (await import('../user-profile/user-profile.module')).UserProfileModule
      },
      {
        path: 'friends',
        loadChildren: async () => (await import('../friends/friends.module')).FriendsModule
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
