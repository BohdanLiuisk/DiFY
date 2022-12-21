import { callFeature } from "@core/calls/store/call/call.reducer";
import { createSelector } from "@ngrx/store";

export const {
  selectCallState,
  selectCall,
  selectParticipants,
  selectLoaded,
  selectLoading,
  selectTestMessage,
  selectCurrentStream,
  selectConnectionData
} = callFeature;
export const selectCallId = createSelector(selectCall, (call) => call?.id);
export const selectCurrentStreamId = createSelector(selectCurrentStream, (stream) => stream?.id);
export const selectMediaTracks = createSelector(selectCurrentStream, (stream) => stream?.getTracks());
