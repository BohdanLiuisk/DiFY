import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class MenuSearchService {
  private _search: Subject<string> = new Subject<string>();

  public search$ = this._search.asObservable();

  public set search(value: string | undefined) {
    this._search.next(value);
  }

  public filter(search?: string, label?: string): boolean {
    if (!search || !label) {
      return false;
    }
    return !label.toLowerCase().includes(search.toLowerCase());
  }
}
