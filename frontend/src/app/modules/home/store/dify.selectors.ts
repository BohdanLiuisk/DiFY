import { createSelector } from '@ngrx/store';
import { difyFeature } from './dify.reducer';

export const { selectHubStatus, selectLayoutConfig } = difyFeature;
export const selectTheme = createSelector(selectLayoutConfig, (config) => config.theme);
export const selectSidebarOpened = createSelector(selectLayoutConfig, (config) => config.sidebarOpened);
