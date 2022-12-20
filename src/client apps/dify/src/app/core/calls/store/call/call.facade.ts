import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { GUID } from "@shared/custom-types";
import { filter, Observable } from "rxjs";
import { callActions } from '@core/calls/store/call/call.actions';
import { selectCall, selectCallId, selectLoaded, selectLoading, selectTestMessage } from "@core/calls/store/call/call.selectors";
import { Call, CallState } from "@core/calls/store/call/call.models";

@Injectable({ providedIn: 'root' })
export class CallFacade {
  public readonly callId$: Observable<GUID> = this.store.select(selectCallId);
  public readonly call$: Observable<Call> = this.store.select(selectCall);
  public readonly loaded$: Observable<boolean> = this.store.select(selectLoaded).pipe(
    filter(loaded => Boolean(loaded))
  );
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly testMessage$: Observable<string> = this.store.select(selectTestMessage);

  constructor(private store: Store<CallState>) { }

  public loadCall(callId: GUID): void {
    this.store.dispatch(callActions.setCallId({ callId }));
    this.store.dispatch(callActions.loadCall());
  }

  public sendTestMessage(): void {
    this.store.dispatch(callActions.testSendMessage({ message: 'aaa' }));
  }
}
