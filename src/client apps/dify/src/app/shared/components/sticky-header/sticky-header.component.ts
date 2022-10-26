import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  distinctUntilChanged,
  filter,
  fromEvent,
  map,
  share,
  throttleTime
} from 'rxjs';

const scrollDirection = {
  up: 'up',
  down: 'down'
};

@Component({
  selector: 'df-sticky-header',
  templateUrl: './sticky-header.component.html',
  styleUrls: ['./sticky-header.component.scss']
})
export class StickyHeaderComponent implements OnInit, AfterViewInit {
  public shrink: boolean = false;

  @Output()
  public shrank: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void { }

  ngAfterViewInit() {
    const scroll$ = fromEvent(window, 'scroll').pipe(
      throttleTime(100),
      map(() => window.scrollY),
      map((y) => (y > 200 ? scrollDirection.down : scrollDirection.up)),
      distinctUntilChanged(),
      share()
    );
    const scrollUp$ = scroll$.pipe(
      filter(direction => direction === 'up')
    );
    const scrollDown$ = scroll$.pipe(
      filter(direction => direction === 'down')
    );
    scrollUp$.subscribe((value) => {
      this.shrink = false;
      this.shrank.emit(false);
    });
    scrollDown$.subscribe((value) => {
      this.shrink = true;
      this.shrank.emit(true);
    });
  }
}
