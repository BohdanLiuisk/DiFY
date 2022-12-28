import { HubConnection, IHttpConnectionOptions, IHubProtocol, IRetryPolicy } from "@microsoft/signalr";
import { BehaviorSubject, from, Observable, share, Subject, throwError } from "rxjs";
import { HubStatus } from "./hub.status";
import { createHubConnection } from "./signalr";

export class SignalrHub {
  private _hubConnection: HubConnection | undefined;
  private _start: Subject<void> = new Subject<void>();
  private _stopped: Subject<void> = new Subject<void>();
  private _hubStatus: BehaviorSubject<HubStatus> = new BehaviorSubject<HubStatus>('unstarted');
  private _error: Subject<any> = new Subject<any>();
  private _closed: Subject<Error | string | undefined> = new Subject<Error | string | undefined>();

  public status$: Observable<HubStatus> = this._hubStatus.asObservable();
  public error$: Observable<any> = this._error.asObservable();
  public stopped$: Observable<void> = this._stopped.asObservable();
  public closed$: Observable<Error | string | undefined> = this._closed.asObservable();

  constructor(
    public hubName: string,
    public hubUrl: string,
    private options: IHttpConnectionOptions | undefined,
    private automaticReconnect: boolean | number[] | IRetryPolicy | undefined,
    private withHubProtocol: IHubProtocol) { }

  public start(): Observable<void> {
    const connection = this._getOpenedConnection();
    connection
      .start()
      .then(() => {
        this._start.next();
        this._hubStatus.next('connected');
      })
      .catch((error) => this._error.next(error));
    return this._start.asObservable();
  }

  public stop(): Observable<void> {
    if (!this._hubConnection) {
      return throwError(() => new Error(`The connection has not been started yet`));
    }
    this._hubConnection
      .stop()
      .then((_) => {
        this._stopped.next();
        this._hubStatus.next('disconnected');
      })
      .catch((error) => this._error.next(error));
    return this._stopped.asObservable();
  }

  public on<T>(eventName: string): Observable<T> {
    return new Observable<T>((observer) => {
      const connection = this._getOpenedConnection();
      const callback = (data: T) => observer.next(data);
      connection.on(eventName, callback);
      const errorSubscription = this._error.subscribe((error) => {
        observer.error(new Error(`Error on ${ eventName }: ${ error }`));
      });
      const stopSubscription = this._stopped.subscribe(() => {
        observer.complete();
      });
      return () => {
        errorSubscription.unsubscribe();
        stopSubscription.unsubscribe();
        connection.off(eventName, callback);
      };
    }).pipe(share());
  }

  send<T extends any>(methodName: string, ...args: any[]) {
    if (!this._hubConnection) {
      return throwError(() => new Error(`The connection has not been started yet`));
    }
    return from(this._hubConnection.invoke<T>(methodName, ...args));
  }

  private _getOpenedConnection(): HubConnection {
    if (!this._hubConnection) {
      this._hubConnection = createHubConnection(this.hubUrl, this.options, this.automaticReconnect, this.withHubProtocol);
      this._hubConnection.onclose((error) => {
        this._closed.next(error ?? `${ this.hubName } closed for some reason`);
      });
      this._hubConnection.onreconnecting(() => {
        this._hubStatus.next('reconnecting');
      });
      this._hubConnection.onreconnected(() => {
        this._hubStatus.next('connected');
      });
    }
    return this._hubConnection;
  }
}
