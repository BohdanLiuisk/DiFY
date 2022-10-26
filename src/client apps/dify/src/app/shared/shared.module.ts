import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  TuiAlertModule, TuiButtonModule,
  TuiDialogModule,
  TuiHintModule, TuiLinkModule,
  TuiRootModule, TuiSvgModule,
  TuiTextfieldControllerModule
} from '@taiga-ui/core';
import { TuiInputModule, TuiInputPasswordModule } from '@taiga-ui/kit';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { StickyHeaderComponent } from './components/sticky-header/sticky-header.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TuiRootModule,
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
  declarations: [
    StickyHeaderComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TuiInputModule,
    TuiTextfieldControllerModule,
    FontAwesomeModule,
    TuiHintModule,
    TuiInputPasswordModule,
    TuiButtonModule,
    StickyHeaderComponent,
    TuiSvgModule,
    TuiLinkModule
  ]
})
export class SharedModule { }
