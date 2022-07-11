import { NgModule } from '@angular/core';
import { SignUpComponent } from './sign-up.component';
import { SignUpRoutingModule } from './sign-up-routing.module';
import { SharedModule } from '@shared/shared.module';
import { SignUpService } from '@core/services/auth/sign-up.service';

@NgModule({
  declarations: [
    SignUpComponent
  ],
  imports: [
    SignUpRoutingModule,
    SharedModule
  ],
  providers: [
    SignUpService
  ]
})
export class SignUpModule { }
