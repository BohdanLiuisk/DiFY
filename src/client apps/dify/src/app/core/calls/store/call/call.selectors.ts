import { callFeature } from "@core/calls/store/call/call.reducer";
import { createSelector } from "@ngrx/store";
import { GUID } from "@shared/custom-types";

export const {
  selectCallState,
  selectCall,
  selectParticipants,
  selectLoaded,
  selectLoading,
  selectCurrentMediaStreamId,
  selectHubConnected,
  selectJoinData,
  selectParticipantCards
} = callFeature;
export const selectCallId = createSelector(selectCall, (call) => call?.id);
export const selectParticipantByStreamId = (streamId: string) =>  createSelector(selectParticipants,
  (participants) => participants.find(participant => participant.streamId === streamId));
export const selectParticipantById = (id: GUID) =>  createSelector(selectParticipants,
  (participants) => participants.find(participant => participant.id === id));