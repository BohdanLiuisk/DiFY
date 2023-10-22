import { GUID } from '@shared/custom-types';

export interface DifyState {
	hubStatus: string;
}

export interface IncomingCallNotification {
	callId: GUID;
	callName: string;
	callerId: GUID;
	callerName: string;
}
