import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CallListComponent } from '@modules/calls/call-list/call-list.component';
import { CallComponent } from '@modules/calls/call/call.component';

const routes: Routes = [
  {
    path: '',
    component: CallListComponent
  },
  {
    path: ':id',
    component: CallComponent
  }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class CallsRoutingModule { }
