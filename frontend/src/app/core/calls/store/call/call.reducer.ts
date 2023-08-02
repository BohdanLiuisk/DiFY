import { createFeature, createReducer, on } from "@ngrx/store";
import { callActions } from '@core/calls/store/call/call.actions';
import { CallParticipantCard, CallState } from "@core/calls/store/call/call.models";

export const callInitialState: CallState = {
  call: null,
  participants: [],
  currentParticipantId: 0,
  currentStream: null,
  currentPeerId: '',
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
    on(callActions.setPeerId, (state, { peerId }) => {
      return { ...state, currentPeerId: peerId };
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
      const currentCard = state.participantCards.find(card => card.currentUser);
      const videoTracks = currentCard.stream.getVideoTracks();
      videoTracks.forEach(track => {
        track.enabled = false;
        track.stop();
      });
      return { ...state, participantCards: [
        ...state.participantCards.map((participantCard): CallParticipantCard => {
          if(participantCard.currentUser) {
            return {
              ...participantCard,
              stream: participantCard.stream,
              videoEnabled: false
            }
          }
          return participantCard;
        })]
      };
    }),
    on(callActions.currentStreamConnected, (state, { stream }) => {
      return { ...state, currentStream: stream };
    }),
    on(callActions.setNewVideoStream, (state, { videoTrack }) => {
      const currentStream = state.participantCards.find(
        card => card.participantId === state.currentParticipantId).stream;
      currentStream.getVideoTracks().forEach((vt) => {
        currentStream.removeTrack(vt);
      });
      currentStream.addTrack(videoTrack);
      return { ...state, participantCards: [
        ...state.participantCards.map((participantCard): CallParticipantCard => {
          if(participantCard.currentUser) {
            return {
              ...participantCard,
              stream: participantCard.stream,
              videoEnabled: true
            }
          }
          return participantCard;
      })]
    };
    }),
    on(callActions.destroyMediaStream, (state) => {
      const currentCard = state.participantCards.find(
        card => card.participantId === state.currentParticipantId);
      if(currentCard && currentCard.stream) {
        currentCard.stream.getTracks().forEach((track) => track.stop());
      }
      return state;
    }),
    on(callActions.addParticipant, (state, participant) => {
      return { ...state, participants: [ ...state.participants, participant ] };
    }),
    on(callActions.removeParticipant, (state, { participantId }) => {
      return { ...state, participants: [ ...state.participants.filter(p => p.participantId !== participantId) ],
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
        const participant = state.participants.find(p => p.streamId === stream.id);
        const currentUser = state.currentParticipantId === participant.participantId;
        return { ...state, participantCards: [ ...state.participantCards, {
          stream,
          participantId: participant.participantId,
          videoEnabled: true,
          audioEnabled: true,
          currentUser,
          participant: state.participants.find(p => p.streamId === stream.id)
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
