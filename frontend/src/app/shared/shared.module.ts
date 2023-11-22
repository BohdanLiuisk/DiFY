import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StickyHeaderComponent } from './components/sticky-header/sticky-header.component';
import { RouterModule } from '@angular/router';
import { DifyLoaderComponent } from './components/dify-loader/dify-loader.component';
import { FormControlValidationComponent } from './components/form-control-validation/form-control-validation.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatIconModule,
    MatButtonModule
  ],
  declarations: [
    StickyHeaderComponent,
    DifyLoaderComponent,
    FormControlValidationComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    StickyHeaderComponent,
    DifyLoaderComponent,
    FormControlValidationComponent,
    RouterModule,
    MatIconModule,
    MatButtonModule
  ]
})
export class SharedModule { }
