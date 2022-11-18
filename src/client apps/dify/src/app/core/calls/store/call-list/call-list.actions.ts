import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { Call, SortOption } from './call-list.reducer';

export const callListActions = createActionGroup({
  source: 'Call List',
  events: {
    'Set List Page': props<{ page: number }>(),
    'Set Per Page': props<({ perPage: number })>(),
    'Add Sort Option': props<({ sortOption: SortOption })>(),
    'Load Calls': emptyProps(),
    'Load Calls Failure': props<{ error: Error }>(),
    'Load Calls Success': props<{ calls: Call[]; totalCount: number }>(),
  }
});
