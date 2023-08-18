import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CallsRoutingModule } from '@modules/calls/calls-routing.module';
import { CallComponent } from './call/call.component';
import { CallListComponent } from './call-list/call-list.component';
import { callListFeature } from '@core/calls/store/call-list/call-list.reducer';
import { CallListEffects } from '@core/calls/store/call-list/call-list.effects';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { CallStatusPipe } from './pipes/call-status.pipe';
import { CallDatePipe } from './pipes/call-date.pipe';
import { callFeature } from '@core/calls/store/call/call.reducer';
import { CallEffects } from '@core/calls/store/call/call.effects';
import { CallParticipantComponent } from './call-participant/call-participant.component';
import { TuiModule } from '@shared/tui.module';
import { CreateNewCallComponent } from './create-new-call/create-new-call.component';

@NgModule({
  declarations: [
    CallComponent,
    CallListComponent,
    CallStatusPipe,
    CallDatePipe,
    CallParticipantComponent,
    CreateNewCallComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    CallsRoutingModule,
    StoreModule.forFeature(callListFeature),
    StoreModule.forFeature(callFeature),
    EffectsModule.forFeature([CallListEffects, CallEffects])
  ]
})
export class CallsModule { }
