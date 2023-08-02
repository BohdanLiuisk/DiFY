import { NgModule } from '@angular/core';
import { WelcomeComponent } from './welcome.component';
import { SharedModule } from '@shared/shared.module';
import { WelcomeRoutingModule } from '@modules/welcome/welcome-routing.module';
import { TuiModule } from '@shared/tui.module';

@NgModule({
  declarations: [
    WelcomeComponent
  ],
  imports: [
    SharedModule,
    WelcomeRoutingModule,
    TuiModule
  ]
})
export class WelcomeModule { }
