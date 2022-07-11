import { GUID } from '@shared/custom-types';

export interface User {
  id: GUID
  name: string,
  email: string,
}
