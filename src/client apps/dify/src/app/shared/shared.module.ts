import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StickyHeaderComponent } from './components/sticky-header/sticky-header.component';
import { RouterModule } from '@angular/router';
import { CallStatusDirective } from './directives/call-status.directive';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  declarations: [
    StickyHeaderComponent,
    CallStatusDirective
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    StickyHeaderComponent,
    RouterModule,
  ]
})
export class SharedModule { }
