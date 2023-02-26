import { NgModule } from '@angular/core';
import {
  TuiButtonModule,
  TuiDialogModule,
  TuiErrorModule,
  TuiHintModule,
  TuiLinkModule,
  TuiSvgModule,
  TuiTextfieldControllerModule
} from '@taiga-ui/core';
import {
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
  TuiTagModule
} from '@taiga-ui/kit';
import { TuiTablePaginationModule } from '@taiga-ui/addon-table';

@NgModule({
  imports: [
    TuiButtonModule,
    TuiTagModule,
    TuiTablePaginationModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiInputModule,
    TuiHintModule,
    TuiSvgModule,
    TuiTextfieldControllerModule,
    TuiLinkModule,
    TuiInputPasswordModule
  ],
  exports: [
    TuiButtonModule,
    TuiTagModule,
    TuiTablePaginationModule,
    TuiDialogModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiInputModule,
    TuiHintModule,
    TuiSvgModule,
    TuiTextfieldControllerModule,
    TuiLinkModule,
    TuiInputPasswordModule
  ]
})
export class TuiModule { }
