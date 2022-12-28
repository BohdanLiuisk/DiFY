import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Call, Participant } from "@core/calls/store/call/call.models";

export const callActions = createActionGroup({
  source: 'Call',
  events: {
    'Set Call Id': props<{ callId: GUID }>(),
    'Start Call Hub': emptyProps(),
    'Call Hub Started': emptyProps(),
    'Get Current Media Stream': emptyProps(),
    'Set Current Media Stream': props<{ mediaStream: MediaStream }>(),
    'Join Call': emptyProps(),
    'Join Call Success': props<{ call: Call; participants: Participant[] }>(),
    'Join Call Failure': props<{ error: any }>(),
    'Set Loading': emptyProps(),
    'Set Loaded': emptyProps(),
    'Set Peer Id': props<{ peerId: string }>(),
    'Set Current Media Stream Id': props<{ streamId: string }>(),
    'Set Join Data': props<{ streamId: string, peerId: string, callId: GUID }>(),
    'Add Participant': props<Participant>(),
    'Remove Participant': props<{ participantId: GUID }>(),
    'Invoke Participant Joined': emptyProps(),
    'Add Participant Card': props<{ stream: MediaStream }>(),
    'Stop Call Hub': emptyProps(),
    'Call Hub Stopped': emptyProps(),
    'Call Hub Info': props<{ info: any }>(),
    'Clear State': emptyProps()
  }
});
