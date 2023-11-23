import { GUID } from '@shared/custom-types';

export interface DifyState {
	hubStatus: string;
	sidebarOpened: boolean;
}

export interface IncomingCallNotification {
	callId: GUID;
	callName: string;
	callerId: GUID;
	callerName: string;
}

export interface MenuItem {
	route: string;
	caption: string;
	icon: string;
}
