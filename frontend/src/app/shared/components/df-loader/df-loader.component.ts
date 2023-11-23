import { Component, Input } from '@angular/core';

@Component({
  selector: 'df-loader',
  templateUrl: './df-loader.component.html',
  styleUrls: ['./df-loader.component.scss']
})
export class DfLoaderComponent {
  @Input() public loading: boolean;

  constructor() { }
}
