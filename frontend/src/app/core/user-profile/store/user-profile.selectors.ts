import { createSelector } from '@ngrx/store';
import { userProfileFeature } from './user-profile.reducer';

export const { selectLoading, selectLoaded, selectError, selectUserProfileState } = userProfileFeature;
export const selectUserProfile = createSelector(selectUserProfileState, (state) => state.user);
