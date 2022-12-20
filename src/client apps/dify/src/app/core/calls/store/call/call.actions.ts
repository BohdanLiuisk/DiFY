import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Call, Participant } from "@core/calls/store/call/call.models";

export const callActions = createActionGroup({
  source: 'Call',
  events: {
    'Set Loaded': emptyProps(),
    'Set Call Id': props<{ callId: GUID }>(),
    'Load Call': emptyProps(),
    'Load Call Failure': props<{ error: Error }>(),
    'Load Call Success': props<{ call: Call; participants: Participant[] }>(),
    'Test Send Message': props<{ message: any }>(),
    'Test Receive Message': props<{ message: any }>()
  }
});
