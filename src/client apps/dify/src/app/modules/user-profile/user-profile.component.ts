import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseComponent } from '@core/components/base.component';
import { User } from '@core/models/user';
import { UserProfileFacade } from '@core/user-profile/user-profile.facade';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent extends BaseComponent implements OnInit, OnDestroy {
  public readonly user$: Observable<User> = this.userProfileFacade.user$
    .pipe(this.untilThis);;
  public readonly isLoading$: Observable<boolean> = this.userProfileFacade.loading$ 
    .pipe(this.untilThis);;
  public readonly loadingError$: Observable<string> = this.userProfileFacade.error$
    .pipe(this.untilThis);

  constructor(
    public userProfileFacade: UserProfileFacade,
    private route: ActivatedRoute) {
    super();
  }

  public ngOnInit(): void {
    this.route.params.pipe(this.untilThis).subscribe(params => {
      const userId: number = params['id'];
      this.userProfileFacade.loadUserProfile(userId);
    });
  }

  public override ngOnDestroy() {
    super.ngOnDestroy();
    this.userProfileFacade.clearState();
  }
}
