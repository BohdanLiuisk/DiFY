export interface UserInfo {
  name: string;
}

export interface UserProfileState {
  user: UserInfo;
  loading: boolean;
}
