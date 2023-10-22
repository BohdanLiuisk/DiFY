import { StoreModule } from '@ngrx/store';
import { AUTH_FEATURE_KEY, authReducer } from '@core/auth/store/auth.reducer';
import { AuthEffects } from '@core/auth/store/auth.effects.';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { AuthFacade } from '@core/auth/store/auth.facade';
import { AuthService, authServiceInitProvider } from '@core/auth/services/auth.service';
import { JWT_OPTIONS, JwtHelperService } from '@auth0/angular-jwt';
import { JwtStorageService } from '@core/auth/services/jwt-storage.service';
import { AuthEventsService } from './services/auth-events.service';

@NgModule({
  imports: [
    StoreModule.forFeature(AUTH_FEATURE_KEY, authReducer),
    EffectsModule.forFeature([AuthEffects])
  ],
  declarations: [],
  providers: [
    AuthEventsService,
    AuthFacade,
    AuthService,
    JwtHelperService,
    JwtStorageService,
    authServiceInitProvider,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS }
  ],
})
export class AuthModule {}
