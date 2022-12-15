import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ComponentStore, OnStoreInit, tapResponse } from "@ngrx/component-store";
import { dify } from "@shared/constans/app-settings";
import { guid, GUID } from "@shared/custom-types";
import { Observable, switchMap, tap, filter } from "rxjs";
import { CallService } from "./call.service";

export interface CallState {
  call: Call,
  participants: Participant[],

  loading: boolean,
  loaded: boolean
}

export interface ConnectionData {
  peerId: string,
  currentUserId: GUID,
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
  loading: false
}

@Injectable()
export class CallStore extends ComponentStore<CallState> implements OnStoreInit {

  constructor(private callService: CallService) {
    super();
  }

  public ngrxOnStoreInit(): void {
    this.loadCall(this.fetchCallDebounce$);
  };

  public readonly call$: Observable<Call> = this.select((state) => state.call);

  public readonly loaded$: Observable<boolean> = this.select((state) => state.loaded)
    .pipe(filter(loaded => Boolean(loaded)));

  public readonly callId$: Observable<GUID> = this.select((state) => state.call.id);

  private readonly fetchCallDebounce$ = this.select(this.callId$, (callId) => ({ callId }), { debounce: true });

  public readonly loadCall = this.effect((callId$: Observable<{callId: GUID}>) => {
    return callId$.pipe(
      tap(() => (this.setLoading())),
      switchMap(({ callId }) => this.callService.getById(callId).pipe(
        tapResponse(
          ({ call, participants }) => {
            this.setCall(call);
            this.setParticipants(participants);
            this.setLoaded();
          },
          (error: HttpErrorResponse) => console.log(error),
        )
      ))
    );
  })

  private readonly setCall = this.updater((state, call: Call) => ({ ...state, call }));

  private readonly setParticipants = this.updater((state, participants: Participant[]) => ({ ...state, participants }));

  private readonly setLoading = this.updater((state) => ({ ...state, loaded: false, loading: true }));

  private readonly setLoaded = this.updater((state) => ({ ...state, loading: false, loaded: true }));
}
