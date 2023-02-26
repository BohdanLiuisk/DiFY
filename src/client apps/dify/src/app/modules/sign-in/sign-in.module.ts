import { NgModule } from '@angular/core';
import { SignInComponent } from './sign-in.component';
import { SignInRoutingModule } from './sign-in-routing.module';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';

@NgModule({
  declarations: [
    SignInComponent
  ],
  imports: [
    SignInRoutingModule,
    SharedModule,
    TuiModule
  ]
})
export class SignInModule { }
