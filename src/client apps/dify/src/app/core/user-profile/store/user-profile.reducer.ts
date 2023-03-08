import { createFeature, createReducer, on } from '@ngrx/store';
import { userProfileActions } from './user-profile.actions';
import { UserProfileState } from './user-profile.models';

const initialState: UserProfileState = {
  user: null,
  loading: false
}

export const userProfileFeature = createFeature({
  name: 'userProfile',
  reducer: createReducer(
    initialState,
    on(userProfileActions.loadProfileSuccess, (state, { user }) => {
      return {
        ...state,
        loading: false,
        user
      };
    }),
    on(userProfileActions.clearState, () => {
      return initialState;
    })
  )
});