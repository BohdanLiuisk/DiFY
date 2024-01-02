import { Injectable } from '@angular/core';
import { FriendsService } from '@modules/friends/services/friends.service';
import { ComponentStore, tapResponse } from '@ngrx/component-store';
import { FindFriendsState } from './find-friends.models';
import { Observable, switchMap } from 'rxjs';
import { concatLatestFrom } from '@ngrx/effects';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class FindFriendsStore extends ComponentStore<FindFriendsState> {

  constructor(private readonly friendsService: FriendsService) {
    super({
      loading: false,
      error: '',
      foundFriends: [],
      totalCount: 0,
      paginationConfig: {
        page: 1,
        perPage: 100
      }
    });
  }

  public readonly error$ = this.select(state => state.error);

  public readonly loading$ = this.select(state => state.loading);

  public readonly paginationConfig$ = this.select(state => state.paginationConfig);

  public readonly foundFriends$ = this.select(state => state.foundFriends);

  public readonly findFriends = this.effect((searchValue$: Observable<string>) => {
    return searchValue$.pipe(
      concatLatestFrom(() => this.paginationConfig$),
      switchMap(([searchValue, { page, perPage }]) => 
        this.friendsService.findFriends(page, perPage, searchValue).pipe(
          tapResponse({
            next: (response) => {
              this.patchState({ 
                foundFriends: response.items,
                totalCount: response.totalCount  
              });
            },
            error: (error: HttpErrorResponse) => {
              console.error(error);
              this.patchState({ error: error.message });
            },
            finalize: () => {
              this.patchState({ loading: false });
            }
          })
        )
      )
    );
  });
}
