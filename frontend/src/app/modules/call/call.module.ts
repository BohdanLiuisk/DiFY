import { CallEffects } from './store/call.effects';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { CallSignalrEventsService } from './services/call-signalr-events.service';
import { CallService } from './services/call.service';
import { CallFacade } from './store/call.facade';
import { CallRoutingModule } from './call-routing.module';
import { callFeature } from './store/call.reducer';
import { CallParticipantComponent } from './components/call-participant/call-participant.component';
import { CallComponent } from './components/call/call.component';

@NgModule({
  declarations: [
    CallParticipantComponent,
    CallComponent
  ],
  imports: [
    TuiModule,
    SharedModule,
    CallRoutingModule,
    StoreModule.forFeature(callFeature),
    EffectsModule.forFeature([CallEffects])
  ],
  providers: [
    CallService,
    CallSignalrEventsService,
    CallFacade
  ]
})
export class CallModule { }
