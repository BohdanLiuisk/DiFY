import { createActionGroup, props } from '@ngrx/store';
import { GUID } from '@shared/custom-types';
import { UserInfo } from './user-profile.models';

export const userProfileActions = createActionGroup({
  source: 'User Profile',
  events: {
    'Load Profile': props<{ id: GUID }>(),
    'Load Profile Success': props<{ user: UserInfo }>(),
    'Load Profile Failed': props<{ error: Error }>()
  }
});
