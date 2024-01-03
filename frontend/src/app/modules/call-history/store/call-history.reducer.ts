import { createFeature, createReducer, on } from '@ngrx/store';
import { CallHistoryConfig, CallHistory, Calls } from '../models/call-history.models';
import { callHistoryActions } from './call-history.actions';

export const callsHistoryInitialState: CallHistory = {
  historyConfig: {
    page: 1,
    perPage: 20
  },
  calls: {
    entities: [],
    loaded: false,
    loading: false,
    totalCount: 0
  },
  newCallCreating: false
};
  
export const callHistoryFeature = createFeature({
  name: 'callHistory',
  reducer: createReducer(
  callsHistoryInitialState,
    on(callHistoryActions.setHistoryPage, (state, { page }) => {
      const historyConfig: CallHistoryConfig = { ...state.historyConfig, page };
      return { ...state, historyConfig };
    }),
    on(callHistoryActions.setPerPage, (state, { perPage }) => {
      const historyConfig: CallHistoryConfig = { ...state.historyConfig, perPage };
      return { ...state, historyConfig };
    }),
    on(callHistoryActions.loadCalls, (state) => {
      const calls: Calls = { ...state.calls, loading: true };
      return { ...state, calls };
    }),
    on(callHistoryActions.loadCallsSuccess, (state, action) => {
      const calls: Calls = {
        ...state.calls,
        entities: action.calls,
        totalCount: action.totalCount,
        loading: false,
        loaded: true,
      };
      return { ...state, calls };
    }),
    on(callHistoryActions.loadCallsFailure, (state, _) => {
      const calls: Calls = {
        ...state.calls,
        entities: [],
        totalCount: 0,
        loading: false,
        loaded: true,
      };
      return { ...state, calls };
    }),
    on(callHistoryActions.createNewCall, (state, _) => {
      return { ...state, newCallCreating: true };
    }),
    on(callHistoryActions.joinCall, (state, _) => {
      return { ...state, newCallCreating: false };
    })
  )
});
