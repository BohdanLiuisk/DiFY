import { Injectable } from '@angular/core';
import { GUID } from '@shared/custom-types';
import { Observable } from 'rxjs';
import { Call, Participant } from '../models/call.models';
import { CoreHttpService } from '@core/services/core-http.service';

@Injectable()
export class CallService {
	private readonly callsPath: string = '/api/calls';

	constructor(private httpService: CoreHttpService) { }

	public getById(callId: GUID): Observable<{ call: Call, participants: Participant[] }> {
		return this.httpService.getRequest<{ call: Call, participants: Participant[] }>(`${ this.callsPath }/${ callId }`);
	}

	public getCanJoin(callId: GUID): Observable<boolean> {
		return this.httpService.getRequest<boolean>(`${ this.callsPath }/getCanJoin/${ callId }`);
	}
}
