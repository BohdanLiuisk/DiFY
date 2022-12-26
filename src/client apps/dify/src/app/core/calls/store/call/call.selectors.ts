import { callFeature } from "@core/calls/store/call/call.reducer";
import { createSelector } from "@ngrx/store";

export const {
  selectCallState,
  selectCall,
  selectParticipants,
  selectLoaded,
  selectLoading,
  selectCurrentMediaStreamId,
  selectConnectionData,
} = callFeature;
export const selectCallId = createSelector(selectCall, (call) => call?.id);
