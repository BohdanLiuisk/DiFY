import { NgModule } from '@angular/core';
import { SignInComponent } from './sign-in.component';
import { SignInRoutingModule } from './sign-in-routing.module';
import { SharedModule } from '@shared/shared.module';
import { TuiButtonModule, TuiHintModule, TuiSvgModule } from '@taiga-ui/core';
import { TuiInputPasswordModule } from '@taiga-ui/kit';

@NgModule({
  declarations: [
    SignInComponent
  ],
  imports: [
    SignInRoutingModule,
    SharedModule,
    TuiHintModule,
    TuiSvgModule,
    TuiInputPasswordModule,
    TuiButtonModule
  ]
})
export class SignInModule { }
