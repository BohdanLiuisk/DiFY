import { GUID } from "@shared/custom-types"

export interface CallState {
  call: Call | null,
  participants: Participant[],
  connectionData: CallConnectionData | null,
  currentMediaStreamId: string,
  loading: boolean,
  loaded: boolean
}

export interface CallConnectionData {
  peerId: string,
  userId: GUID,
  callId: GUID,
  streamId: string,
  participant: Participant
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
  joinOn: Date
}
