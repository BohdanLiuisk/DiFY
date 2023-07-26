import { callFeature } from "@core/calls/store/call/call.reducer";
import { createSelector } from "@ngrx/store";
import { GUID } from "@shared/custom-types";

export const {
  selectCallState,
  selectCall,
  selectParticipants,
  selectLoaded,
  selectLoading,
  selectHubConnected,
  selectParticipantCards,
  selectCurrentParticipantId
} = callFeature;
export const selectCallId = createSelector(selectCall, (call) => call?.id);
export const selectParticipantByStreamId = (streamId: string) =>  createSelector(selectParticipants,
  (participants) => participants.find(participant => participant.streamId === streamId));
export const selectParticipantById = (id: number) =>  createSelector(selectParticipants,
  (participants) => participants.find(participant => participant.id === id));
export const selectCurrentParticipantCard = createSelector(selectParticipantCards, selectCurrentParticipantId,
  (participantCards, currentParticipantId) => participantCards.find(card => card.participantId === currentParticipantId));
export const selectCurrentMediaStream = createSelector(selectCurrentParticipantCard, (card) => card.stream);
export const selectParticipantsCount = createSelector(selectParticipantCards, (cards) => cards.length);
