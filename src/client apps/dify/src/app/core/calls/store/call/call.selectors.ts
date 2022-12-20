import { callFeature } from "@core/calls/store/call/call.reducer";
import { createSelector } from "@ngrx/store";

export const { selectCallState, selectCall, selectParticipants, selectLoaded, selectLoading, selectTestMessage } = callFeature;
export const selectCallId = createSelector(selectCall, (call) => call.id);
