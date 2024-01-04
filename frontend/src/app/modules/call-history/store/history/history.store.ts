import { CallHistoryService } from '../../services/call-history.service';
import { CallHistoryState } from './history-store.models';
import { Injectable } from '@angular/core';
import { ComponentStore, tapResponse } from '@ngrx/component-store';
import { switchMap, tap, withLatestFrom } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { BaseCallHistoryStore } from '../base/base.store';
import { callsSections } from '../../models/call-history.models';

@Injectable()
export class CallHistoryStore extends ComponentStore<CallHistoryState> {

  constructor(
    private readonly callHistoryService: CallHistoryService,
    private readonly baseStore: BaseCallHistoryStore) {
    super({
      loading: false,
      error: '',
      calls: [],
      totalCount: 0,
      paginationConfig: {
        page: 1,
        perPage: 10
      }
    });
  }

  public readonly paginationConfig$ = this.select(state => state.paginationConfig);

  public readonly callsHistory$ = this.select(state => state.calls);

  public readonly loading$ = this.select(state => state.loading);

  public readonly setHistoryPage = this.updater((state, page: number) => ({
    ...state, paginationConfig: { ...state.paginationConfig, page }
  }));

  public readonly setPerPage = this.updater((state, perPage: number) => ({
    ...state, paginationConfig: { ...state.paginationConfig, perPage }
  }));

  public readonly loadHistory = this.effect<void>((trigger$) => trigger$.pipe(
    withLatestFrom(this.paginationConfig$),
    tap(() => {
      this.patchState({ loading: true });
      this.baseStore.setSectionLoading(callsSections.history);
    }),
    switchMap(([_, { page, perPage}]) => this.callHistoryService.getAll(page, perPage).pipe(
      tapResponse({
        next: (response) => {
          this.patchState({ 
            calls: response.items,
            totalCount: response.totalCount  
          });
        },
        error: (error: HttpErrorResponse) => {
          console.error(error);
          this.patchState({ error: error.message });
        },
        finalize: () => {
          this.patchState({ loading: false });
          this.baseStore.setSectionLoaded(callsSections.history);
        }
      })
    ))
  ));
}
