import { NgModule } from '@angular/core';
import { SignUpComponent } from './sign-up.component';
import { SignUpRoutingModule } from './sign-up-routing.module';
import { SharedModule } from '@shared/shared.module';
import { MaterialModule } from '@shared/material.module';

@NgModule({
  declarations: [
    SignUpComponent
  ],
  imports: [
    SignUpRoutingModule,
    SharedModule,
    MaterialModule
  ],
  providers: [ ]
})
export class SignUpModule { }
