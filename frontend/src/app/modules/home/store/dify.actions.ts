import { IncomingCallEvent } from '../models/dify.models';
import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { GUID } from '@shared/custom-types';

export const difyActions = createActionGroup({
  source: 'Dify',
  events: {
    'Successfully Authenticated': emptyProps(),
    'Dify Hub Started': emptyProps(),
    'Dify Hub Status Changed': props<{ status: string }>(),
    'Incoming Call Notification': props<IncomingCallEvent>(),
    'Join Incoming Call': props<{ callId: GUID }>(),
    'Decline Incoming Call': props<{ callId: GUID }>(),
    'Toggle Sidebar': emptyProps(),
    'Switch Theme': emptyProps()
  }
});
