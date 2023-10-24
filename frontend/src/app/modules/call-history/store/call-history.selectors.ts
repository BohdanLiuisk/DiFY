import { callHistoryFeature } from './call-history.reducer';
import { createSelector } from '@ngrx/store';

export const { selectCallHistoryState, selectCalls, selectHistoryConfig, selectNewCallCreating } = callHistoryFeature;
export const selectCallEntities = createSelector(selectCalls, (calls) => calls.entities);
export const selectCallsTotalCount = createSelector(selectCalls, (calls) => calls.totalCount);
export const selectIsLoading = createSelector(selectCalls, (calls) => calls.loading);
