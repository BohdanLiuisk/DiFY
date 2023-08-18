import { NgModule } from '@angular/core';
import {
  TuiAlertModule,
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
  TuiAvatarModule,
  TuiDataListWrapperModule,
  TuiFieldErrorPipeModule,
  TuiInputModule,
  TuiInputPasswordModule,
  TuiMultiSelectModule,
  TuiSelectModule,
  TuiTagModule
} from '@taiga-ui/kit';
import { TuiTablePaginationModule } from '@taiga-ui/addon-table';
import { TuiActiveZoneModule, TuiLetModule } from '@taiga-ui/cdk';

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
    TuiDataListWrapperModule,
    TuiActiveZoneModule,
    TuiDataListModule,
    TuiDropdownModule,
    TuiMultiSelectModule,
    TuiLetModule,
    TuiAvatarModule,
    TuiAlertModule,
    TuiSelectModule
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
    TuiDataListWrapperModule,
    TuiActiveZoneModule,
    TuiDataListModule,
    TuiDropdownModule,
    TuiMultiSelectModule,
    TuiLetModule,
    TuiAvatarModule,
    TuiSelectModule,
    TuiAlertModule
  ]
})
export class TuiModule { }
