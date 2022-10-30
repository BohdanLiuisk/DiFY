import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CallsRoutingModule } from '@modules/calls/calls-routing.module';
import { CallComponent } from './call/call.component';
import { CallListComponent } from './call-list/call-list.component';

@NgModule({
  declarations: [
    CallComponent,
    CallListComponent
  ],
  imports: [
    SharedModule,
    CallsRoutingModule
  ]
})
export class CallsModule { }
