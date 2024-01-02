import { FoundFriend } from '@modules/friends/models/friends.models';

export interface FindFriendsState {
  loading: boolean;
  error: string;
  foundFriends: FoundFriend[];
  totalCount: number;
  paginationConfig: {
    page: number;
    perPage: number;
  }
}
