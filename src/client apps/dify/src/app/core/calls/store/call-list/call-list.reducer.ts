import { GUID } from '@shared/custom-types';
import { createFeature, createReducer, on } from '@ngrx/store';
import { callListActions } from './call-list.actions';

export interface CallList {
  listConfig: CallListConfig,
  calls: Calls
}

export interface CallListConfig {
  page: number;
  perPage: number;
}

export interface Calls {
  entities: Call[],
  totalCount: number,
  loading: boolean,
  loaded: boolean
}

export interface Call {
  id: GUID,
  name: string,
  active: boolean,
  startDate: Date,
  endDate: Date,
  activeParticipants: number,
  totalParticipants: number
}

export const callsListInitialState: CallList = {
  listConfig: {
    page: 1,
    perPage: 10
  },
  calls: {
    entities: [],
    loaded: false,
    loading: false,
    totalCount: 0
  }
}

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
