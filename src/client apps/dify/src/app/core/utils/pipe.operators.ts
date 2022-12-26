import { Observable } from "rxjs";

export const filterEmpty = () => {
  return function<T>(source: Observable<T>): Observable<T> {
    return new Observable(subscriber => {
      source.subscribe({
        next(value) {
          if(Boolean(value)) {
            subscriber.next(value);
          }
        },
        error(error) {
          subscriber.error(error);
        },
        complete() {
          subscriber.complete();
        }
      })
    });
  }
}
