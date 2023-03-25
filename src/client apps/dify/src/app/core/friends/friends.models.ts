import { GUID } from '@shared/custom-types';

export interface FriendCategory {
  id: number;
  name: string;
}

export interface FriendTile {
  id:GUID;
  name: string;
  avatarUrl: string;
  online: boolean;
  workplace: string;
  tags: FriendTag[];
}

export interface FriendRequestTile {
  id:GUID;
  name: string;
  avatarUrl: string;
  direction: string;
}

export interface FriendTag {
  name: string;
  color: string;
}

export interface MayKnownMan {
  id: string;
  name: string;
  avatarUrl: string;
  online: boolean;
  mutualFriends: number;
}