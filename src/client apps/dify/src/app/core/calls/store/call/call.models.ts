import { GUID } from "@shared/custom-types"

export interface CallState {
  call: Call | null,
  participants: Participant[],
  participantCards: CallParticipantCard[],
  currentParticipantId: number,
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
  participantId: number,
  name: string,
  active: false,
  streamId: string,
  peerId: string,
  joinedAt: Date
}

export interface CallParticipantCard {
  participantId: number,
  stream: MediaStream,
  videoEnabled: boolean,
  audioEnabled: boolean,
  currentUser: boolean,
  participant: Participant
}
