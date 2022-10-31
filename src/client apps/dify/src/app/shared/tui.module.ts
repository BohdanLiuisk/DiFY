import { NgModule } from '@angular/core';
import {
  TuiAlertModule, TuiButtonModule,
  TuiDialogModule, TuiHintModule, TuiLinkModule, TuiLoaderModule,
  TuiSvgModule, TuiTextfieldControllerModule
} from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule } from '@taiga-ui/kit';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TuiTableModule, TuiTablePaginationModule } from '@taiga-ui/addon-table';
import { TuiLetModule } from '@taiga-ui/cdk';

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
    TuiLinkModule,
    TuiTableModule,
    TuiLetModule,
    TuiLoaderModule,
    TuiTablePaginationModule
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
    TuiLinkModule,
    TuiTableModule,
    TuiLetModule,
    TuiLoaderModule,
    TuiTablePaginationModule
  ]
})
export class TuiModule { }
