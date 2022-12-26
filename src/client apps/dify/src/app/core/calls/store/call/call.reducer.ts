import { createFeature, createReducer, on } from "@ngrx/store";
import { dify } from "@shared/constans/app-settings";
import { callActions } from '@core/calls/store/call/call.actions';
import { CallState } from "@core/calls/store/call/call.models";

export const callInitialState: CallState = {
  call: null,
  participants: [],
  currentMediaStreamId: dify.emptyString,
  connectionData: null,
  loaded: false,
  loading: false
}

export const callFeature = createFeature({
  name: 'call',
  reducer: createReducer(
    callInitialState,
    on(callActions.setCallId, (state, { callId }) => {
      return { ...state, call: { ...state.call, id: callId } };
    }),
    on(callActions.setCurrentMediaStreamId, (state, { streamId }) => {
      return { ...state, currentMediaStreamId: streamId };
    }),
    on(callActions.setConnectionData, (state, { peerId, callId, userId, streamId }) => {
      return { ...state, connectionData: { ...state.connectionData, peerId, callId, userId, streamId } };
    }),
    on(callActions.addParticipant, (state, participant) => {
      return { ...state, participants: [ ...state.participants, participant ] };
    }),
    on(callActions.removeParticipant, (state, { participantId }) => {
      return { ...state, participants: [ ...state.participants.filter(p => p.id === participantId) ] };
    }),
    on(callActions.loadCall, (state) => {
      return { ...state, loading: true, loaded: false };
    }),
    on(callActions.loadCallSuccess, (state, { call, participants }) => {
      return { ...state, call, participants };
    }),
    on(callActions.setLoaded, (state) => {
      return { ...state, loading: false, loaded: true };
    }),
    on(callActions.clearState, () => {
      return callInitialState;
    })
  )
});
