import { createFeature, createReducer, on } from '@ngrx/store';
import { userProfileActions } from './user-profile.actions';
import { UserProfileState } from '../models/user-profile.models';

const initialState: UserProfileState = {
  user: null,
  loading: false,
  loaded: false,
  error: null
}

export const userProfileFeature = createFeature({
  name: 'userProfile',
  reducer: createReducer(
    initialState,
    on(userProfileActions.loadProfile, (state) => {
      return {
        ...state,
        loading: true
      };
    }),
    on(userProfileActions.loadProfileSuccess, (state, { user }) => {
      return {
        ...state,
        loading: false,
        loaded: true,
        user
      };
    }),
    on(userProfileActions.loadProfileFailed, (state, { error }) => {
      return {
        ...state,
        loading: false,
        loaded: true,
        error: error.message
      };
    }),
    on(userProfileActions.clearState, () => {
      return initialState;
    })
  )
});
