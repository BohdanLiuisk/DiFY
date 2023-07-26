import { User } from '@core/models/user';
import { createActionGroup, emptyProps, props } from '@ngrx/store';

export const userProfileActions = createActionGroup({
  source: 'User Profile',
  events: {
    'Load Profile': props<{ id: number }>(),
    'Load Profile Success': props<{ user: User }>(),
    'Load Profile Failed': props<{ error: Error }>(),
    'Clear State': emptyProps()
  }
});
