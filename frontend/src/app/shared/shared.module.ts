import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StickyHeaderComponent } from './components/sticky-header/sticky-header.component';
import { RouterModule } from '@angular/router';
import { DifyLoaderComponent } from './components/dify-loader/dify-loader.component';
import { FormControlValidationComponent } from './components/form-control-validation/form-control-validation.component';
import { DfMultiSelectComponent } from './components/df-multiselect-dropdown/df-multiselect-dropdown.component';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatIconModule
  ],
  declarations: [
    StickyHeaderComponent,
    DifyLoaderComponent,
    FormControlValidationComponent,
    DfMultiSelectComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    StickyHeaderComponent,
    DifyLoaderComponent,
    DfMultiSelectComponent,
    FormControlValidationComponent,
    RouterModule
  ]
})
export class SharedModule { }
