import { Injectable } from "@angular/core";
import { CoreHttpService } from "@core/services/core-http.service";
import { GUID } from "@shared/custom-types";
import { Observable } from "rxjs";
import { Call, Participant } from "@core/calls/store/call/call.models";

@Injectable({ providedIn: 'root' })
export class CallService {
	private readonly callsPath: string = '/api/social/calls';

	constructor(private httpService: CoreHttpService) { }

	public getById(callId: GUID): Observable<{ call: Call, participants: Participant[] }> {
		return this.httpService.getRequest<{ call: Call, participants: Participant[] }>(`${ this.callsPath }/${ callId }`);
	}
}
