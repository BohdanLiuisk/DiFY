import { GUID, Theme } from '@shared/custom-types';

export interface DifyState {
	hubStatus: string;
	layoutConfig: LayoutConfig;
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

export interface LayoutConfig {
	theme: Theme;
	ripple: boolean;
	inputFilled: boolean;
	sidebarOpened: boolean;
}
