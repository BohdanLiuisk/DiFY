import { GUID } from "@shared/custom-types";

export interface UserInfo {
  id: GUID;
  firstName: string;
  lastName: string;
  login: string;
  email: string;
  createdOn: Date;
  avatarUrl: string;
}

export interface UserProfileState {
  user: UserInfo;
  loading: boolean;
}
