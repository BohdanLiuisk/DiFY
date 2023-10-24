import { NgModule } from '@angular/core';
import { CreateNewCallComponent } from './components/create-new-call/create-new-call.component';
import { TuiModule } from '@shared/tui.module';
import { SharedModule } from '@shared/shared.module';
import { CallHistoryComponent } from './components/call-history/call-history.component';
import { CallHistoryStatusPipe } from './pipes/call-history-status.pipe';
import { CallHistoryDatePipe } from './pipes/call-history-date.pipe';
import { callHistoryFeature } from './store/call-history.reducer';
import { CallHistoryEffects } from './store/call-history.effects';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { CallHistoryService } from './services/call-history.service';
import { CallHistoryFacade } from './store/call-history.facade';
import { CallHistoryRoutingModule } from './call-history-routing.module';

@NgModule({
  declarations: [
    CallHistoryComponent,
    CreateNewCallComponent,
    CallHistoryStatusPipe,
    CallHistoryDatePipe,
  ],
  imports: [
    TuiModule,
    SharedModule,
    CallHistoryRoutingModule,
    StoreModule.forFeature(callHistoryFeature),
    EffectsModule.forFeature([CallHistoryEffects])
  ],
  providers: [
    CallHistoryService,
    CallHistoryFacade
  ]
})
export class CallHistoryModule { }
