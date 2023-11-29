import { createFeature, createReducer, on } from '@ngrx/store';
import { DifyState } from '../models/dify.models';
import { difyActions } from './dify.actions';
import { Theme } from '@shared/custom-types';

const difyInitialState: DifyState =  {
  hubStatus: '',
  sidebarOpened: true,
  theme: 'light'
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
    }),
    on(difyActions.switchTheme, (state) => {
      const theme: Theme = state.theme === 'dark' ? 'light': 'dark';
      return { ...state, theme };
    }),
  )
});
