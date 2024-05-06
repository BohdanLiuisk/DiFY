import { Injectable } from '@angular/core';
import { CoreHttpService } from '@core/services/core-http.service';
import { GUID } from '@shared/custom-types';
import { Observable } from 'rxjs';
import { CanJoinCall } from '../models/dify.models';

@Injectable()
export class HomeService {
	private readonly callsPath: string = '/api/calls';

	constructor(private httpService: CoreHttpService) { }

	public getCanJoinCall(callId: GUID): Observable<CanJoinCall> {
		return this.httpService.getRequest<CanJoinCall>(`${ this.callsPath }/getCanJoin/${ callId }`);
	}
}
