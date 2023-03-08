import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseComponent } from '@core/components/base.component';
import { UserInfo } from '@core/user-profile/store/user-profile.models';
import { UserProfileFacade } from '@core/user-profile/user-profile.facade';
import { GUID } from '@shared/custom-types';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent extends BaseComponent implements OnInit, OnDestroy {
  public readonly user$: Observable<UserInfo> = this.userProfileFacade.user$;

  constructor(
    public userProfileFacade: UserProfileFacade,
    private route: ActivatedRoute) {
    super();
  }

  public ngOnInit(): void {
    this.route.params.pipe(this.untilThis).subscribe(params => {
      const userId: GUID = params['id'];
      this.userProfileFacade.loadUserProfile(userId);
    });
  }

  public override ngOnDestroy() {
    super.ngOnDestroy();
    this.userProfileFacade.clearState();
  }
}
