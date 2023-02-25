import { NgModule } from '@angular/core';
import { TuiButtonModule, TuiDialogModule, TuiErrorModule, TuiHintModule } from '@taiga-ui/core';
import { TuiFieldErrorPipeModule, TuiInputModule, TuiTagModule } from '@taiga-ui/kit';
import { TuiTablePaginationModule } from '@taiga-ui/addon-table';

@NgModule({
  imports: [
    TuiButtonModule,
    TuiTagModule,
    TuiTablePaginationModule,
    TuiDialogModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiInputModule,
    TuiHintModule
  ],
  exports: [
    TuiButtonModule,
    TuiTagModule,
    TuiTablePaginationModule,
    TuiDialogModule,
    TuiErrorModule,
    TuiFieldErrorPipeModule,
    TuiInputModule,
    TuiHintModule
  ]
})
export class TuiModule { }
