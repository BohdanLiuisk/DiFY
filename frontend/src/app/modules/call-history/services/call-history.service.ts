import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { Observable } from 'rxjs';
import { GUID } from '@shared/custom-types';
import { CallsResponse, CreateNewCallConfig, ParticipantForCall } from '../models/call-history.models';

@Injectable()
export class CallHistoryService {
  private readonly callsPath: string = '/api/calls';

  constructor(private httpService: CoreHttpService) { }

  public getAll(page: number, perPage: number): Observable<CallsResponse> {
    return this.httpService.getRequest<CallsResponse>(
      `${this.callsPath}/getCalls?pageNumber=${page}&pageSize=${perPage}`);
  }

  public createNew(newCallConfig: CreateNewCallConfig): Observable<{ callId: GUID }> {
    return this.httpService.postRequest<{ callId: GUID }>(`${this.callsPath}/createNew`, newCallConfig);
  }

  public joinCall(callId: GUID): Observable<void> {
    return this.httpService.putRequest<void>(`${this.callsPath}/joinCall/${callId}`);
  }

  public searchParticipants(search: string | undefined): Observable<ParticipantForCall[]> {
    return this.httpService.getRequest<ParticipantForCall[]>(
      `${this.callsPath}/searchParticipants?searchValue=${search}`);
  }
}
