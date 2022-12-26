import { HubConnectionBuilder, IHttpConnectionOptions, IHubProtocol, IRetryPolicy } from "@microsoft/signalr";
import { SignalrHub } from "./signalr.hub";

const hubs: SignalrHub[] = [];

export const createHubConnection = (
  url: string,
  options?: IHttpConnectionOptions | undefined,
  automaticReconnect?: boolean | number[] | IRetryPolicy | undefined,
  withHubProtocol?: IHubProtocol
) => {
  const builder = new HubConnectionBuilder();
  if (options) {
    builder.withUrl(url, options);
  } else {
    builder.withUrl(url);
  }
  if (automaticReconnect === true) {
    builder.withAutomaticReconnect();
  }
  if (automaticReconnect instanceof Array) {
    builder.withAutomaticReconnect(automaticReconnect);
  }
  if (typeof automaticReconnect === "object" && "nextRetryDelayInMilliseconds" in automaticReconnect) {
    builder.withAutomaticReconnect(automaticReconnect);
  }
  if (withHubProtocol) {
    builder.withHubProtocol(withHubProtocol);
  }
  return builder.build();
};

export const createHub = (hubName: string, url: string, options?: IHttpConnectionOptions | undefined,
  automaticReconnect?: boolean | number[] | IRetryPolicy | undefined, withHubProtocol?: IHubProtocol): SignalrHub | undefined => {
  const hub = new SignalrHub(hubName, url, options, automaticReconnect, withHubProtocol);
  hubs.push(hub);
  return hub;
};

export function findHub(hub: { hubName: string, hubUrl: string }): SignalrHub | undefined {
  return hubs.filter((h) => h.hubName === hub.hubName && h.hubUrl === hub.hubUrl)[0];
}

export function removeHub(hub: SignalrHub): void {
  const index = hubs.indexOf(hub);
  hubs.splice(index, 1);
}
