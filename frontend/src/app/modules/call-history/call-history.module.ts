import { NgModule } from '@angular/core';
import { CreateNewCallComponent } from './components/create-new-call/create-new-call.component';
import { TuiModule } from '@shared/tui.module';
import { SharedModule } from '@shared/shared.module';
import { CallHistoryComponent } from './components/call-history/call-history.component';
import { callHistoryFeature } from './store/call-history.reducer';
import { CallHistoryEffects } from './store/call-history.effects';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { CallHistoryService } from './services/call-history.service';
import { CallHistoryFacade } from './store/call-history.facade';
import { CallHistoryRoutingModule } from './call-history-routing.module';
import { 
  DfMultiSelectComponent 
} from '@shared/components/df-multiselect-dropdown/df-multiselect-dropdown.component';
import { DividerModule } from 'primeng/divider';

@NgModule({
  declarations: [
    CallHistoryComponent,
    CreateNewCallComponent
  ],
  imports: [
    TuiModule,
    SharedModule,
    CallHistoryRoutingModule,
    DfMultiSelectComponent,
    DividerModule,
    StoreModule.forFeature(callHistoryFeature),
    EffectsModule.forFeature([CallHistoryEffects])
  ],
  providers: [
    CallHistoryService,
    CallHistoryFacade
  ]
})
export class CallHistoryModule { }
