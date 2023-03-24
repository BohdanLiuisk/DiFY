import { NgModule } from '@angular/core';
import {
  TuiButtonModule,
  TuiDataListModule,
  TuiDialogModule, TuiDropdownModule,
  TuiErrorModule,
  TuiGroupModule,
  TuiHintModule,
  TuiHostedDropdownModule,
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
import { TuiActiveZoneModule } from '@taiga-ui/cdk';

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
    TuiInputPasswordModule,
    TuiGroupModule,
    TuiHostedDropdownModule,
    TuiActiveZoneModule,
    TuiDataListModule,
    TuiDropdownModule
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
    TuiInputPasswordModule,
    TuiGroupModule,
    TuiHostedDropdownModule,
    TuiActiveZoneModule,
    TuiDataListModule,
    TuiDropdownModule
  ]
})
export class TuiModule { }
