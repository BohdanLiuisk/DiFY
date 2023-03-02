import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { SocialRoutingModule } from './social-routing.module';

@NgModule({
  declarations: [],
  imports: [
    SocialRoutingModule,
    SharedModule,
    TuiModule
  ]
})
export class SocialModule { }
