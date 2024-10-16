import { GUID } from '@shared/custom-types';

export interface CreateNewCallConfig {
  name: string;
  participantIds: number[];
}

export interface ParticipantForCall {
  id: number;
  name: string;
  userName: string;
  avatarUrl: string;
  isOnline: boolean;
}

export interface CallHistory {
  historyConfig: CallHistoryPaginationConfig,
  calls: Calls,
  newCallCreating: boolean
};

export interface CallHistoryPaginationConfig {
  page: number;
  perPage: number;
};

export interface Calls {
  entities: Call[];
  totalCount: number;
  loading: boolean;
  loaded: boolean;
};

export enum CallDirection {
  Incoming,
  Outgoing
}

export enum CallParticipantStatus {
  Active,
  NotActive,
  Completed,
  NotAnswered,
  Declined,
  CanceledByCaller,
  Missed,
  Failed
}

export interface CallParticipant {
  id: number;
  callParticipantId: number;
  name: string;
  avatarUrl: string;
  isOnline: boolean;
  direction: CallDirection;
  status: CallParticipantStatus
}

export interface Call {
  id: GUID;
  name: string;
  active: boolean;
  initiatorId: number;
  startDate: Date;
  endDate: Date;
  duration: number;
  direction: CallDirection;
  status: CallParticipantStatus;
  participants: CallParticipant[];
};

export interface CallsResponse {
  items: Call[];
  totalCount: number;
}

export interface CallHistorySection {
  code: string;
  caption: string;
  route: string;
  routeExact: boolean;
  icon: string;
  loading: boolean;
  enabled: boolean;
  color: string;
}

export const callsSections = {
  history: 'history',
  callFriends: 'call-friends',
  active: 'active',
  missed: 'missed',
  sceduled: 'sceduled'
};
