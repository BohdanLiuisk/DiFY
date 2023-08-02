import { Injectable } from '@angular/core';
import { filterEmpty } from '@core/utils/pipe.operators';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { userProfileActions } from './store/user-profile.actions';
import { UserProfileState } from './store/user-profile.models';
import { selectError, selectLoading, selectUserProfile } from './store/user-profile.selectors';
import { User } from '@core/models/user';

@Injectable({ providedIn: 'root' })
export class UserProfileFacade {
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly user$: Observable<User> = this.store.select(selectUserProfile).pipe(filterEmpty());
  public readonly error$: Observable<string> = this.store.select(selectError);

  constructor(private store: Store<UserProfileState>) { }

  public loadUserProfile(id: number) {
    this.store.dispatch(userProfileActions.loadProfile({ id }));
  }

  public clearState() {
    this.store.dispatch(userProfileActions.clearState());
  }
}
