import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { Call, CreateNewCallConfig } from '../models/call-history.models';
import { GUID } from '@shared/custom-types';

export const callHistoryActions = createActionGroup({
  source: 'Call History',
  events: {
    'Create New Call': props<CreateNewCallConfig>(),
    'Join Call': props<{ callId: GUID }>(),
    'Set History Page': props<{ page: number }>(),
    'Set Per Page': props<({ perPage: number })>(),
    'Load Calls': emptyProps(),
    'Load Calls Failure': props<{ error: Error }>(),
    'Load Calls Success': props<{ calls: Call[]; totalCount: number }>(),
  }
});
