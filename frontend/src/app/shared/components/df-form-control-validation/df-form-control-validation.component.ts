import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'df-form-control-validation',
  templateUrl: './df-form-control-validation.component.html',
  styleUrls: ['./df-form-control-validation.component.scss']
})
export class DfFormControlValidationComponent {
  @Input('control') 
  public control: AbstractControl;
  
  @Input('caption') 
  public caption: string;
}
