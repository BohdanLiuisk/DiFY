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

export enum CallColumns {
  name = 'Name',
  startDate = 'StartDate',
  active = 'Active',
  duration = 'Duration',
  activeParticipants = 'ActiveParticipants',
  totalParticipants = 'TotalParticipants'
};

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

export interface Call {
  id: GUID;
  name: string;
  active: boolean;
  initiatorId: number;
  startDate: Date;
  endDate: Date;
  duration: number;
  activeParticipants: number;
  totalParticipants: number;
};

export interface CallsResponse {
  items: Call[];
  totalCount: number;
}
