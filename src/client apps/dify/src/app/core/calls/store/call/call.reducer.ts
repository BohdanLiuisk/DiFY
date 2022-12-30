import { createFeature, createReducer, on } from "@ngrx/store";
import { callActions } from '@core/calls/store/call/call.actions';
import { CallParticipantCard, CallState } from "@core/calls/store/call/call.models";
import { dify } from "@shared/constans/app-settings";
import { guid } from "@shared/custom-types";

export const callInitialState: CallState = {
  call: null,
  participants: [],
  currentParticipantId: guid(dify.emptyString),
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
    on(callActions.setCurrentParticipantId, (state, { id }) => {
      return { ...state, currentParticipantId: id };
    }),
    on(callActions.callHubStarted, (state, {  }) => {
      return { ...state, hubConnected: true };
    }),
    on(callActions.joinCallSuccess, (state, { call, participants }) => {
      return { ...state, call, participants };
    }),
    on(callActions.stopVideoStream, (state) => {
      const videoTracks = state.participantCards
        .find(card => card.participantId === state.currentParticipantId).stream.getVideoTracks();
      videoTracks.forEach(track => {
        track.enabled = false;
        track.stop();
      });
      return state;
    }),
    on(callActions.setNewVideoStream, (state, { videoTrack }) => {
      const currentStream = state.participantCards.find(
        card => card.participantId === state.currentParticipantId).stream;
      currentStream.getVideoTracks().forEach((vt) => {
        currentStream.removeTrack(vt);
      });
      currentStream.addTrack(videoTrack);
      return state;
    }),
    on(callActions.addParticipant, (state, participant) => {
      return { ...state, participants: [ ...state.participants, participant ] };
    }),
    on(callActions.removeParticipant, (state, { participantId }) => {
      return { ...state, participants: [ ...state.participants.filter(p => p.id !== participantId) ],
        participantCards: [ ...state.participantCards
          .filter(s => s.participantId !== participantId)
          .map((participantCard): CallParticipantCard => ({
            ...participantCard,
            stream: participantCard.stream
          }))
        ]
      };
    }),
    on(callActions.addParticipantCard, (state, { stream }) => {
      if(!state.participantCards.some(p => p.stream.id === stream.id)) {
        return { ...state, participantCards: [ ...state.participantCards, {
          stream,
          participantId: state.participants.find(p => p.streamId === stream.id).id,
          videoEnabled: true,
          audioEnabled: true
        }]};
      } else {
        return state;
      }
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
