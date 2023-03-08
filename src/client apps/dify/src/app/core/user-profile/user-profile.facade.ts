import { Injectable } from '@angular/core';
import { filterEmpty } from '@core/utils/pipe.operators';
import { Store } from '@ngrx/store';
import { GUID } from '@shared/custom-types';
import { Observable } from 'rxjs';
import { userProfileActions } from './store/user-profile.actions';
import { UserInfo, UserProfileState } from './store/user-profile.models';
import { selectLoading, selectUser } from './store/user-profile.selectors';

@Injectable({ providedIn: 'root' })
export class UserProfileFacade {
  public readonly loading$: Observable<boolean> = this.store.select(selectLoading);
  public readonly user$: Observable<UserInfo> = this.store.select(selectUser).pipe(filterEmpty());

  constructor(private store: Store<UserProfileState>) { }

  public loadUserProfile(id: GUID) {
    this.store.dispatch(userProfileActions.loadProfile({ id }));
  }

  public clearState() {
    this.store.dispatch(userProfileActions.clearState());
  }
}
