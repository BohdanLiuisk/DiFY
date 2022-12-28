import { GUID } from "@shared/custom-types"

export interface CallState {
  call: Call | null,
  participants: Participant[],
  joinData: JoinData,
  currentMediaStream: MediaStream,
  currentMediaStreamId: string,
  participantCards: CallParticipantCard[],
  hubConnected: boolean,
  loading: boolean,
  loaded: boolean
}

export interface JoinData {
  peerId: string,
  streamId: string,
  callId: GUID
}

export interface Call {
  id: GUID,
  name: string,
  startDate: Date,
  totalParticipants: number,
  activeParticipants: number
}

export interface Participant {
  id: GUID,
  name: string,
  active: false,
  streamId: string,
  peerId: string,
  joinOn: Date
}

export interface CallParticipantCard {
  participantId: GUID,
  stream: MediaStream
}
