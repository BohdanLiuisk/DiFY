import { GUID } from "@shared/custom-types";

export interface UserInfo {
  id: GUID;
  firstName: string;
  lastName: string;
  login: string;
  email: string;
  createdOn: Date;
  avatarUrl: string;
  currentUser: boolean;
  online: boolean;
}

export interface UserProfileState {
  user: UserInfo;
  loading: boolean;
}
