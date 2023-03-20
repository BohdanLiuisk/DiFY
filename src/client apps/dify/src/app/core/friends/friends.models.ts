import {GUID} from '@shared/custom-types';

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

export interface FriendTag {
  name: string;
  color: string;
}
