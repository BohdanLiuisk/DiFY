import { GUID, Theme } from '@shared/custom-types';

export interface DifyState {
	hubStatus: string;
	layoutConfig: LayoutConfig;
}

export interface CallerInfo {
	id: number;
	name: string
	avatarUrl: string;
}

export interface IncomingCallEvent {
	id: GUID;
	name: string;
	caller: CallerInfo;
	otherParticipants?: CallerInfo[];
}

export interface MenuItem {
	route: string;
	caption: string;
	icon: string;
	exact: boolean;
}

export interface LayoutConfig {
	theme: Theme;
	ripple: boolean;
	inputFilled: boolean;
	sidebarOpened: boolean;
}

export interface CanJoinCall {
	success: boolean;
	errorMessage: string;
}
