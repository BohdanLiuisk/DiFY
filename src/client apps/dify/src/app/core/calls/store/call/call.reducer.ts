import { createFeature, createReducer, on } from "@ngrx/store";
import { dify } from "@shared/constans/app-settings";
import { guid } from "@shared/custom-types";
import { callActions } from '@core/calls/store/call/call.actions';
import { CallState } from "@core/calls/store/call/call.models";

export const callInitialState: CallState = {
  call: {
    id: guid(dify.emptyString),
    name: dify.emptyString,
    startDate: new Date(),
    totalParticipants: 0,
    activeParticipants: 0
  },
  participants: [],
  loaded: false,
  loading: false,
  testMessage: dify.emptyString
}

export const callFeature = createFeature({
  name: 'call',
  reducer: createReducer(
    callInitialState,
    on(callActions.setCallId, (state, { callId }) => {
      return { ...state, call: { ...state.call, id: callId } };
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
    on(callActions.testReceiveMessage, (state, { message }) => {
      return { ...state, testMessage: message.userId };
    })
  )
});
