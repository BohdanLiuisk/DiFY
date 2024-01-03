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
  historyConfig: CallHistoryConfig,
  calls: Calls,
  newCallCreating: boolean
};

export interface CallHistoryConfig {
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
  Canceled,
  Missed,
  Incoming,
  Outgoing,
  Declined
}

export interface CallParticipant {
  id: number;
  callParticipantId: number;
  name: string;
  avatarUrl: string;
  isOnline: boolean;
  direction: CallDirection;
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
  participants: CallParticipant[];
};

export interface CallsResponse {
  items: Call[];
  totalCount: number;
}
