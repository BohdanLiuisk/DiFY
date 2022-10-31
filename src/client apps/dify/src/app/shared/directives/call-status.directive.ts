import { Directive, HostBinding, Input } from '@angular/core';

@Directive({
  selector: '[dfCallStatus]'
})
export class CallStatusDirective {
  @Input() active: boolean = false;

  constructor() { }

}
