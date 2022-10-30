import { NgModule } from '@angular/core';
import { WelcomeComponent } from './welcome.component';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { WelcomeRoutingModule } from '@modules/welcome/welcome-routing.module';

@NgModule({
  declarations: [
    WelcomeComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    WelcomeRoutingModule
  ]
})
export class WelcomeModule { }
