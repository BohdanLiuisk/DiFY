import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { GUID } from '@shared/custom-types';
import { Call } from './call-list.reducer';

export const callListActions = createActionGroup({
  source: 'Call List',
  events: {
    'Create New Call': props<{ name: string }>(),
    'Join Call': props<{ callId: GUID }>(),
    'Set List Page': props<{ page: number }>(),
    'Set Per Page': props<({ perPage: number })>(),
    'Load Calls': emptyProps(),
    'Load Calls Failure': props<{ error: Error }>(),
    'Load Calls Success': props<{ calls: Call[]; totalCount: number }>(),
  }
});
