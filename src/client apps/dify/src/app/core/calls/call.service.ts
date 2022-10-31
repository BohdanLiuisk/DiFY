import { Injectable } from "@angular/core";
import { CoreHttpService } from '@core/services/core-http.service';
import { Observable } from 'rxjs';
import { Call } from '@core/calls/store/call-list/call-list.reducer';

@Injectable({ providedIn: 'root' })
export class CallService {
  private readonly callsPath: string = '/api/social/calls';

  constructor(private httpService: CoreHttpService) { }

  public getAll(page: number, perPage: number): Observable<{ calls: Call[], totalCount: number }> {
    return this.httpService.getRequest<{ calls: Call[], totalCount: number }>(
      `${this.callsPath}/getAll?page=${page}&perPage=${perPage}`);
  }
}
