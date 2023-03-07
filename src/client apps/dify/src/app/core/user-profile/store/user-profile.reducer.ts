import { createFeature, createReducer, on } from '@ngrx/store';
import { dify } from '@shared/constans/app-settings';
import { userProfileActions } from './user-profile.actions';
import { UserProfileState } from './user-profile.models';

const initialState: UserProfileState = {
  user: {
    name: dify.emptyString
  },
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
    })
  )
});
