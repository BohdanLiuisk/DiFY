import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-control-validation',
  templateUrl: './form-control-validation.component.html',
  styleUrls: ['./form-control-validation.component.scss']
})
export class FormControlValidationComponent {
  @Input('control') 
  public control: AbstractControl;
  
  @Input('caption') 
  public caption: string;
}
