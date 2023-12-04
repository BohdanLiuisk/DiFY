import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { FeedRoutingModule } from './feed-routing.module';
import { FeedComponent } from './components/feed/feed.component';
import { 
  DfMultiSelectComponent 
} from '@shared/components/df-multiselect-dropdown/df-multiselect-dropdown.component';
import { CallHistoryService } from '@modules/call-history/services/call-history.service';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';

@NgModule({
  declarations: [
    FeedComponent
  ],
  imports: [
    FeedRoutingModule,
    SharedModule,
    TuiModule,
    DfMultiSelectComponent,
    InputTextModule,
    ButtonModule
  ],
  providers: [
    CallHistoryService
  ]
})
export class FeedModule { }
