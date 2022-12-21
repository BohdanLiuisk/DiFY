import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Call, CallConnectionData, Participant } from "@core/calls/store/call/call.models";
import { Observable } from "rxjs";

export const callActions = createActionGroup({
  source: 'Call',
  events: {
    'Set Loaded': emptyProps(),
    'Set Call Id': props<{ callId: GUID }>(),
    'Set Current Media Stream': props<{ stream: Observable<MediaStream> }>(),
    'Get Current Media Stream Success': props<{ stream: MediaStream }>(),
    'Set Peer Id': props<{ peerId: string }>(),
    'Set Connection Data': props<CallConnectionData>(),
    'Load Call': emptyProps(),
    'Load Call Failure': props<{ error: Error }>(),
    'Load Call Success': props<{ call: Call; participants: Participant[] }>(),
    'Test Send Message': props<{ message: any }>(),
    'Test Receive Message': props<{ message: any }>()
  }
});
