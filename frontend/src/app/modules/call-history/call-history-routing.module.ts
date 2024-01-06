import { CallHistoryComponent } from './components/base/base.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoryComponent } from './components/history/history.component';
import { CallFriendsComponent } from './components/call-friends/call-friends.component';

const routes: Routes = [
  {
    path: '',
    component: CallHistoryComponent,
    children: [
      {
        path: '',
        component: HistoryComponent
      },
      {
        path: 'call-friends',
        component: CallFriendsComponent
      },
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class CallHistoryRoutingModule { }
