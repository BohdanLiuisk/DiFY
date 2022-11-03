import { NgModule } from '@angular/core';
import { WelcomeComponent } from './welcome.component';
import { SharedModule } from '@shared/shared.module';
import { WelcomeRoutingModule } from '@modules/welcome/welcome-routing.module';
import { MaterialModule } from '@shared/material.module';

@NgModule({
  declarations: [
    WelcomeComponent
  ],
  imports: [
    SharedModule,
    MaterialModule,
    WelcomeRoutingModule
  ]
})
export class WelcomeModule { }
