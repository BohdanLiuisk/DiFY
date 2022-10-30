import { NgModule } from '@angular/core';
import { SignUpComponent } from './sign-up.component';
import { SignUpRoutingModule } from './sign-up-routing.module';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';

@NgModule({
  declarations: [
    SignUpComponent
  ],
  imports: [
    SignUpRoutingModule,
    SharedModule,
    TuiModule
  ],
  providers: [ ]
})
export class SignUpModule { }
