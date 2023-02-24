import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StickyHeaderComponent } from './components/sticky-header/sticky-header.component';
import { RouterModule } from '@angular/router';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  declarations: [
    StickyHeaderComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    StickyHeaderComponent,
    RouterModule
  ]
})
export class SharedModule { }
