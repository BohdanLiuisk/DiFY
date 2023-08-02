import { Injectable } from "@angular/core";
import { CoreHttpService } from '@core/services/core-http.service';
import { Observable } from 'rxjs';
import { CallsResponse } from '@core/calls/store/call-list/call-list.reducer';
import { GUID } from "@shared/custom-types";

@Injectable({ providedIn: 'root' })
export class CallListService {
  private readonly callsPath: string = '/api/calls';

  constructor(private httpService: CoreHttpService) { }

  public getAll(page: number, perPage: number): Observable<CallsResponse> {
    return this.httpService.getRequest<CallsResponse>(
      `${this.callsPath}/getCalls?pageNumber=${page}&pageSize=${perPage}`);
  }

  public createNew(name: string): Observable<{ callId: GUID }> {
    return this.httpService.postRequest<{ callId: GUID }>(`${this.callsPath}/createNew`, { name });
  }

  public joinCall(callId: GUID): Observable<void> {
    return this.httpService.putRequest<void>(`${this.callsPath}/joinCall/${callId}`);
  }
}
