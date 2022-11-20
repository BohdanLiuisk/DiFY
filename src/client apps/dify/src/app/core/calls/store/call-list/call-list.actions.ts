import { HttpErrorResponse } from '@angular/common/http';
import { BusinessRuleException } from '@core/types/business-rule-exception';
import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { GUID } from '@shared/custom-types';
import { Call, SortOption } from './call-list.reducer';

export const callListActions = createActionGroup({
  source: 'Call List',
  events: {
    'Create New Call': props<{ name: string }>(),
    'Join Call': props<{ callId: GUID }>(),
    'Join Call Failure': props<{ error: HttpErrorResponse }>(),
    'Join Call Success': props<{ callId: GUID }>(),
    'Set List Page': props<{ page: number }>(),
    'Set Per Page': props<({ perPage: number })>(),
    'Add Sort Option': props<({ sortOption: SortOption })>(),
    'Load Calls': emptyProps(),
    'Load Calls Failure': props<{ error: Error }>(),
    'Load Calls Success': props<{ calls: Call[]; totalCount: number }>(),
  }
});
