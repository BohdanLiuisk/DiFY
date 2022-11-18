import { Component, OnDestroy } from "@angular/core";
import { Observable, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-base',
  template: '',
})
export class BaseComponent implements OnDestroy {
  protected destroyed$: Subject<void> = new Subject<void>();

  public readonly untilThis = <T>(source: Observable<T>) => source.pipe(takeUntil(this.destroyed$));

  public ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
