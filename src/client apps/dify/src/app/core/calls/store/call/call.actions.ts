import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { Call, Participant } from "@core/calls/store/call/call.models";

export const callActions = createActionGroup({
  source: 'Call',
  events: {
    'Set Call Id': props<{ callId: GUID }>(),
    'Start Call Hub': emptyProps(),
    'Call Hub Started': emptyProps(),
    'Stop Video Stream': emptyProps(),
    'Enable Video Stream': emptyProps(),
    'Destroy Media Stream': emptyProps(),
    'Set New Video Stream': props<{ videoTrack: MediaStreamTrack }>(),
    'Set Current Participant Id': props<{ id: GUID }>(),
    'Join Call': props<{ peerId: string }>(),
    'Join Call Success': props<{ call: Call; participants: Participant[], stream: MediaStream }>(),
    'Join Call Failure': props<{ error: any }>(),
    'Set Loading': emptyProps(),
    'Set Loaded': emptyProps(),
    'Set Peer Id': props<{ peerId: string }>(),
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
