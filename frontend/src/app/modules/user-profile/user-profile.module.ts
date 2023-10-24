import { NgModule } from '@angular/core';
import { UserProfileEffects } from '@modules/user-profile/store/user-profile.effects';
import { userProfileFeature } from '@modules/user-profile/store/user-profile.reducer';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { UserProfileRoutingModule } from './user-profile-routing.module';
import { UserProfileComponent } from './components/user-profile.component';
import { UserProfileService } from './services/user-profile.service';
import { UserProfileFacade } from './store/user-profile.facade';

@NgModule({
  declarations: [
    UserProfileComponent
  ],
  imports: [
    SharedModule,
    TuiModule,
    UserProfileRoutingModule,
    StoreModule.forFeature(userProfileFeature),
    EffectsModule.forFeature([UserProfileEffects])
  ],
  providers: [
    UserProfileService,
    UserProfileFacade
  ]
})
export class UserProfileModule { }
