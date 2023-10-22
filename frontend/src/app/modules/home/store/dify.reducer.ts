import { createFeature, createReducer, on } from '@ngrx/store';
import { DifyState } from './dify.models';
import { difyActions } from './dify.actions';

const difyInitialState: DifyState =  {
  hubStatus: ''
};

export const difyFeature = createFeature({
  name: 'dify',
  reducer: createReducer(
    difyInitialState,
    on(difyActions.difyHubStatusChanged, (state, { status }) => {
      return { ...state, hubStatus: status };
    })
  )
});
