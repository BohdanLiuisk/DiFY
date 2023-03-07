import { NgModule } from '@angular/core';
import { UserProfileEffects } from '@core/user-profile/store/user-profile.effects';
import { userProfileFeature } from '@core/user-profile/store/user-profile.reducer';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { UserProfileRoutingModule } from './user-profile-routing.module';
import { UserProfileComponent } from './user-profile.component';

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
  ]
})
export class UserProfileModule { }
