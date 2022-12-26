import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Call, Participant } from "@core/calls/store/call/call.models";

export const callActions = createActionGroup({
  source: 'Call',
  events: {
    'Set Call Id': props<{ callId: GUID }>(),
    'Load Call': emptyProps(),
    'Load Call Failure': props<{ error: Error }>(),
    'Load Call Success': props<{ call: Call; participants: Participant[] }>(),
    'Set Loaded': emptyProps(),
    'Set Peer Id': props<{ peerId: string }>(),
    'Set Current Media Stream Id': props<{ streamId: string }>(),
    'Set Connection Data': props<{ peerId: string, callId: GUID, streamId: string, userId: GUID }>(),
    'Add Participant': props<Participant>(),
    'Remove Participant': props<{ participantId: GUID }>(),
    'Invoke Participant Joined': props<{ peerId: string, callId: GUID, streamId: string, userId: GUID }>(),
    'Call Hub Started': emptyProps(),
    'Stop Call Hub': emptyProps(),
    'Call Hub Stopped': props<{ result: string }>(),
    'Call Hub Error': props<{ error: any }>(),
    'Clear State': emptyProps()
  }
});
