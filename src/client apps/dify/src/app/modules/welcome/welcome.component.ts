import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { faSignIn, faArrowRight }  from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WelcomeComponent implements OnInit {
  public faSignIn = faSignIn;
  public faArrowRight = faArrowRight;

  constructor() { }

  ngOnInit(): void { }
}
