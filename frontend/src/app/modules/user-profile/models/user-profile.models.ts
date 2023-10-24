import { User } from '@core/models/user';

export interface UserProfileState {
  user: User;
  loading: boolean;
  loaded: boolean;
  error: string;
}
