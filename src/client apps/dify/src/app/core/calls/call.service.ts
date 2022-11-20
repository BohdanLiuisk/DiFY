import { Injectable } from "@angular/core";
import { CoreHttpService } from '@core/services/core-http.service';
import { Observable } from 'rxjs';
import { Call, SortOptions } from '@core/calls/store/call-list/call-list.reducer';
import { GUID } from "@shared/custom-types";

@Injectable({ providedIn: 'root' })
export class CallService {
  private readonly callsPath: string = '/api/social/calls';

  constructor(private httpService: CoreHttpService) { }

  public getAll(page: number, perPage: number, sortOptions: SortOptions): Observable<{ calls: Call[], totalCount: number }> {
    return this.httpService.postRequest<{ calls: Call[], totalCount: number }>(
      `${this.callsPath}/getAll?page=${page}&perPage=${perPage}`, sortOptions);
  }

  public createNew(name: string): Observable<{ callId: GUID }> {
    return this.httpService.postRequest<{ callId: GUID }>(`${this.callsPath}/createCall`, { name });
  }
}
