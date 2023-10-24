import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CallComponent } from './components/call/call.component';

const routes: Routes = [
  {
    path: ':id',
    component: CallComponent
  }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class CallRoutingModule { }
