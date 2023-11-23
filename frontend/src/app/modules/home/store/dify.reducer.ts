import { createFeature, createReducer, on } from '@ngrx/store';
import { DifyState } from '../models/dify.models';
import { difyActions } from './dify.actions';

const difyInitialState: DifyState =  {
  hubStatus: '',
  sidebarOpened: true
};

export const difyFeature = createFeature({
  name: 'dify',
  reducer: createReducer(
    difyInitialState,
    on(difyActions.difyHubStatusChanged, (state, { status }) => {
      return { ...state, hubStatus: status };
    }),
    on(difyActions.toggleSidebar, (state) => {
      return { ...state, sidebarOpened: !state.sidebarOpened };
    })
  )
});
