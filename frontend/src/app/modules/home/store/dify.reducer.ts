import { createFeature, createReducer, on } from '@ngrx/store';
import { DifyState } from '../models/dify.models';
import { difyActions } from './dify.actions';
import { Theme } from '@shared/custom-types';

const difyInitialState: DifyState =  {
  hubStatus: '',
  layoutConfig: {
    theme: 'light',
    ripple: true,
    inputFilled: false,
    sidebarOpened: true
  }
};

export const difyFeature = createFeature({
  name: 'dify',
  reducer: createReducer(
    difyInitialState,
    on(difyActions.difyHubStatusChanged, (state, { status }) => {
      return { ...state, hubStatus: status };
    }),
    on(difyActions.toggleSidebar, (state) => {
      return { 
        ...state, 
        layoutConfig: { 
          ...state.layoutConfig, 
          sidebarOpened: !state.layoutConfig.sidebarOpened 
        }
      };
    }),
    on(difyActions.switchTheme, (state) => {
      const theme: Theme = state.layoutConfig.theme === 'dark' ? 'light': 'dark';
      return { 
        ...state,
        layoutConfig: { 
          ...state.layoutConfig, 
          theme
        }
      };
    }),
  )
});
