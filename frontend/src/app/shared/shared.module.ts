import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DfLoaderComponent } from './components/df-loader/df-loader.component';
import { DfFormControlValidationComponent } from './components/df-form-control-validation/df-form-control-validation.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  declarations: [
    DfLoaderComponent,
    DfFormControlValidationComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    DfLoaderComponent,
    DfFormControlValidationComponent
  ]
})
export class SharedModule { }
