import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CallsRoutingModule } from '@modules/calls/calls-routing.module';
import { CallComponent } from './call/call.component';
import { CallListComponent } from './call-list/call-list.component';
import { TuiModule } from '@shared/tui.module';
import { callListFeature } from '@core/calls/store/call-list/call-list.reducer';
import { CallListEffects } from '@core/calls/store/call-list/call-list.effects';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

@NgModule({
  declarations: [
    CallComponent,
    CallListComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    CallsRoutingModule,
    StoreModule.forFeature(callListFeature),
    EffectsModule.forFeature([CallListEffects]),
  ]
})
export class CallsModule { }
