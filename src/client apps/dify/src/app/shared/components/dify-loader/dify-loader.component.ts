import { Component, Input } from '@angular/core';

@Component({
  selector: 'dify-loader',
  templateUrl: './dify-loader.component.html',
  styleUrls: ['./dify-loader.component.scss']
})
export class DifyLoaderComponent {
  @Input() public loading: boolean;

  constructor() { }
}
