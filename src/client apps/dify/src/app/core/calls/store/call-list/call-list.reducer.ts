import { GUID } from '@shared/custom-types';
import { createFeature, createReducer, on } from '@ngrx/store';
import { callListActions } from './call-list.actions';

export enum CallColumns {
  name = 'Name',
  startDate = 'StartDate',
  active = 'Active',
  duration = 'Duration',
  activeParticipants = 'ActiveParticipants',
  totalParticipants = 'TotalParticipants'
};

export enum SortDirection {
  asc = 'ASC',
  desc = 'DESC'
};

export interface SortOption {
  column: string,
  direction: SortDirection,
  seqNumber: number
};

export type SortOptions = SortOption[];

export interface CallList {
  listConfig: CallListConfig,
  calls: Calls
};

export interface CallListConfig {
  page: number;
  perPage: number;
  sortOptions: SortOptions;
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
  startDate: Date;
  endDate: Date;
  activeParticipants: number;
  totalParticipants: number;
};

export const callsListInitialState: CallList = {
  listConfig: {
    page: 1,
    perPage: 10,
    sortOptions: []
  },
  calls: {
    entities: [],
    loaded: false,
    loading: false,
    totalCount: 0
  }
};

export const callListFeature = createFeature({
  name: 'callList',
  reducer: createReducer(
    callsListInitialState,
    on(callListActions.setListPage, (state, { page }) => {
      const listConfig: CallListConfig = { ...state.listConfig, page };
      return { ...state, listConfig };
    }),
    on(callListActions.setPerPage, (state, { perPage }) => {
      const listConfig: CallListConfig = { ...state.listConfig, perPage };
      return { ...state, listConfig };
    }),
    on(callListActions.addSortOption, (state, { sortOption }) => {
      const listConfig: CallListConfig = {...state.listConfig, sortOptions: [
        ...state.listConfig.sortOptions.filter(so => so.column !== sortOption.column), sortOption
      ]};
      return { ...state, listConfig };
    }),
    on(callListActions.loadCalls, (state) => {
      const calls: Calls = { ...state.calls, loading: true };
      return { ...state, calls };
    }),
    on(callListActions.loadCallsSuccess, (state, action) => {
      const calls: Calls = {
        ...state.calls,
        entities: action.calls,
        totalCount: action.totalCount,
        loading: false,
        loaded: true,
      };
      return { ...state, calls };
    }),
    on(callListActions.loadCallsFailure, (state, _) => {
      const calls: Calls = {
        ...state.calls,
        entities: [],
        totalCount: 0,
        loading: false,
        loaded: true,
      };
      return { ...state, calls };
    })
  )
});
