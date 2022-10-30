import { NgModule } from '@angular/core';
import {
  TuiAlertModule, TuiButtonModule,
  TuiDialogModule, TuiHintModule, TuiLinkModule,
  TuiSvgModule,  TuiTextfieldControllerModule
} from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule } from '@taiga-ui/kit';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  imports: [
    TuiDialogModule,
    TuiAlertModule,
    TuiInputModule,
    TuiTextfieldControllerModule,
    TuiHintModule,
    TuiInputPasswordModule,
    FontAwesomeModule,
    TuiButtonModule,
    TuiSvgModule,
    TuiLinkModule
  ],
  exports: [
    TuiAlertModule,
    TuiDialogModule,
    TuiInputModule,
    TuiTextfieldControllerModule,
    FontAwesomeModule,
    TuiHintModule,
    TuiInputPasswordModule,
    TuiButtonModule,
    TuiSvgModule,
    TuiLinkModule
  ]
})
export class TuiModule { }
