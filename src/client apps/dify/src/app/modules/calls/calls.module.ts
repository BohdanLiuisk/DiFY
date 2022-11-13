import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CallsRoutingModule } from '@modules/calls/calls-routing.module';
import { CallComponent } from './call/call.component';
import { CallListComponent } from './call-list/call-list.component';
import { callListFeature } from '@core/calls/store/call-list/call-list.reducer';
import { CallListEffects } from '@core/calls/store/call-list/call-list.effects';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MaterialModule } from '@shared/material.module';
import { CallStatusPipe } from './pipes/call-status.pipe';
import { CallDatePipe } from './pipes/call-date.pipe';

@NgModule({
  declarations: [
    CallComponent,
    CallListComponent,
    CallStatusPipe,
    CallDatePipe
  ],
  imports: [
    SharedModule,
    MaterialModule,
    CallsRoutingModule,
    StoreModule.forFeature(callListFeature),
    EffectsModule.forFeature([CallListEffects]),
  ]
})
export class CallsModule { }
