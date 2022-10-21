import { AfterViewInit, Component, OnInit } from '@angular/core';
import { distinctUntilChanged, filter, fromEvent, map, pairwise, share, tap, throttleTime } from 'rxjs';

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

  constructor() { }

  ngOnInit(): void { }

  ngAfterViewInit() {
    const scroll$ = fromEvent(window, 'scroll').pipe(
      throttleTime(100),
      map(() => window.scrollY),
      filter((y) => y > 200),
      pairwise(),
      map(([y1, y2]) => (y2 < y1 ? scrollDirection.up : scrollDirection.down)),
      distinctUntilChanged(),
      share()
    );
    const scrollUp$ = scroll$.pipe(
      filter(direction => direction === 'up')
    );
    const scrollDown$ = scroll$.pipe(
      filter(direction => direction === 'down')
    );
    scrollUp$.subscribe(() => {
      this.shrink = false;
    });
    scrollDown$.subscribe(() => {
      this.shrink = true;
    });
  }
}
