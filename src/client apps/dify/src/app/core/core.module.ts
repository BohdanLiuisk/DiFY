import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AuthTokenInterceptor } from '@core/interceptors/auth-token.interceptor';
import { JwtStorageService } from '@core/services/auth/jwt-storage.service';
import { JWT_OPTIONS, JwtHelperService } from '@auth0/angular-jwt';

@NgModule({
  imports: [
    HttpClientModule,
    SharedModule
  ],
  exports: [ ],
  declarations: [ ],
  providers: [
    JwtStorageService,
    JwtHelperService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthTokenInterceptor, multi: true },
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
  ]
})
export class CoreModule { }
