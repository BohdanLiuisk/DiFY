import { Injectable } from "@angular/core";
import { ComponentStore } from "@ngrx/component-store";
import { GUID } from "@shared/custom-types";

export interface CallState {
  call: Call,
  loading: boolean,
  loaded: boolean
}

export interface Call {
  id: GUID,
  name: string,
  participants: Participant[],
}

export interface Participant {
  id: GUID,
  name: string,
  active: false
}

@Injectable()
export class CallStore extends ComponentStore<CallState> {

  constructor() { super(); }


}
