import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WelcomeComponent } from './welcome.component';
import { RouterModule } from '@angular/router';
import { WelcomeRoutingModule } from './welcome-routing.module';
import { SharedModule } from '@shared/shared.module';
import { TuiTabsModule } from '@taiga-ui/kit';
import { TuiButtonModule, TuiSvgModule } from '@taiga-ui/core';

@NgModule({
  declarations: [
    WelcomeComponent
  ],
    imports: [
        CommonModule,
        RouterModule,
        WelcomeRoutingModule,
        SharedModule,
        TuiTabsModule,
        TuiSvgModule,
        TuiButtonModule
    ]
})
export class WelcomeModule { }
