import { GUID } from "@shared/custom-types"

export interface CallState {
  call: Call,
  participants: Participant[],
  loading: boolean,
  loaded: boolean,
  testMessage: string
}

export interface CallConnectionData {
  peerId: string,
  userId: GUID,
  streamId: string
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
