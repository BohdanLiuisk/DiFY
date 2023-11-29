import { GUID, Theme } from '@shared/custom-types';

export interface DifyState {
	hubStatus: string;
	sidebarOpened: boolean;
	theme: Theme;
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
