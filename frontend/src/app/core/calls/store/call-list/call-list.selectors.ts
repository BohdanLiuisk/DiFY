import { callListFeature } from '@core/calls/store/call-list/call-list.reducer';
import { createSelector } from '@ngrx/store';

export const { selectCallListState, selectCalls, selectListConfig, selectNewCallCreating } = callListFeature;
export const selectCallEntities = createSelector(selectCalls, (calls) => calls.entities);
export const selectCallsTotalCount = createSelector(selectCalls, (calls) => calls.totalCount);
export const selectIsLoading = createSelector(selectCalls, (calls) => calls.loading);
