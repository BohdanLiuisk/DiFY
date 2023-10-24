import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AuthModule } from '@core/auth/auth.module';
import { AuthInterceptor } from '@core/auth/auth.interceptor';
import { TuiModule } from '@shared/tui.module';
import { CoreHttpService } from './services/core-http.service';
import { NavigationService } from './services/navigation.service';

@NgModule({
  imports: [
    HttpClientModule,
    SharedModule,
    AuthModule,
    TuiModule
  ],
  exports: [],
  declarations: [],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    CoreHttpService,
    NavigationService
  ]
})
export class CoreModule { }
