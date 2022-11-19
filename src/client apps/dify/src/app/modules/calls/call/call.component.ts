import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss']
})
export class CallComponent implements OnInit {

  constructor(private route: ActivatedRoute)
    { }

  public ngOnInit(): void {
    this.route.params.subscribe(params => {

    });
  }
}
