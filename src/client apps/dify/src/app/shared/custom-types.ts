export type Token = 'access_token' | 'refresh_token';

export type GUID = string & { isGuid: true};
function guid(guid: string) : GUID {
  return guid as GUID;
}
