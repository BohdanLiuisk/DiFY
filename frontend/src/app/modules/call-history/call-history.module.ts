import { NgModule } from '@angular/core';
import { DialogModule } from '@angular/cdk/dialog';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { SharedModule } from '@shared/shared.module';
import { 
  DfMultiSelectComponent 
} from '@shared/components/df-multiselect-dropdown/df-multiselect-dropdown.component';
import { CallHistoryComponent } from './components/base/base.component';
import { NewCallFormComponent } from './components/new-call-form/new-call-form.component';
import { CallHistoryRoutingModule } from './call-history-routing.module';
import { BaseCallHistoryStore } from './store/base/base.store';
import { CallHistoryService } from './services/call-history.service';

@NgModule({
  declarations: [
    CallHistoryComponent,
    NewCallFormComponent
  ],
  imports: [
    SharedModule,
    CallHistoryRoutingModule,
    DfMultiSelectComponent,
    DividerModule,
    ButtonModule,
    DynamicDialogModule,
    DialogModule,
    InputTextModule
  ],
  providers: [
    CallHistoryService,
    BaseCallHistoryStore
  ]
})
export class CallHistoryModule { }
