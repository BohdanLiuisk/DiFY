import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { GUID } from '@shared/custom-types';
import { Call } from './call-list.reducer';
import { CreateNewCallConfig } from './call-list.models';

export const callListActions = createActionGroup({
  source: 'Call List',
  events: {
    'Create New Call': props<CreateNewCallConfig>(),
    'Join Call': props<{ callId: GUID }>(),
    'Decline Incoming Call': props<{ callId: GUID }>(),
    'Set List Page': props<{ page: number }>(),
    'Set Per Page': props<({ perPage: number })>(),
    'Load Calls': emptyProps(),
    'Load Calls Failure': props<{ error: Error }>(),
    'Load Calls Success': props<{ calls: Call[]; totalCount: number }>(),
  }
});
