import { createFeature, createReducer, on } from "@ngrx/store";
import { dify } from "@shared/constans/app-settings";
import { callActions } from '@core/calls/store/call/call.actions';
import { CallParticipantCard, CallState } from "@core/calls/store/call/call.models";

export const callInitialState: CallState = {
  call: null,
  participants: [],
  currentMediaStream: null,
  currentMediaStreamId: dify.emptyString,
  joinData: null,
  participantCards: [],
  loaded: false,
  loading: false,
  hubConnected: false
}

export const callFeature = createFeature({
  name: 'call',
  reducer: createReducer(
    callInitialState,
    on(callActions.setCallId, (state, { callId }) => {
      return { ...state, call: { ...state.call, id: callId } };
    }),
    on(callActions.callHubStarted, (state, {  }) => {
      return { ...state, hubConnected: true };
    }),
    on(callActions.joinCallSuccess, (state, { call, participants }) => {
      return { ...state, call, participants, joinedCall: true };
    }),
    on(callActions.setCurrentMediaStreamId, (state, { streamId }) => {
      return { ...state, currentMediaStreamId: streamId };
    }),
    on(callActions.setJoinData, (state, { streamId, peerId, callId }) => {
      return { ...state, joinData: { ...state.joinData, streamId, peerId, callId } };
    }),
    on(callActions.setCurrentMediaStream, (state, { mediaStream }) => {
      return { ...state, currentMediaStream: mediaStream.clone() };
    }),
    on(callActions.addParticipant, (state, participant) => {
      return { ...state, participants: [ ...state.participants, participant ] };
    }),
    on(callActions.removeParticipant, (state, { participantId }) => {
      return { ...state, participants: [ ...state.participants.filter(p => p.id === participantId) ],
        participantCards: [ ...state.participantCards
          .filter(s => s.participantId !== participantId)
          .map((participantCard):CallParticipantCard => ({
            stream: participantCard.stream.clone(),
            participantId: participantCard.participantId
          }))
        ]
      };
    }),
    on(callActions.addParticipantCard, (state, { stream }) => {
      return { ...state, participantCards: [ ...state.participantCards, {
        stream,
        participantId: state.participants.find(p => p.streamId === stream.id).id
      }]};
    }),
    on(callActions.setLoading, (state) => {
      return { ...state, loading: true, loaded: false };
    }),
    on(callActions.setLoaded, (state) => {
      return { ...state, loading: false, loaded: true };
    }),
    on(callActions.clearState, () => {
      return callInitialState;
    })
  )
});
