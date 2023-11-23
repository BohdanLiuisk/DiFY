import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthModule } from '@core/auth/auth.module';
import { AuthInterceptor } from '@core/auth/auth.interceptor';
import { TuiModule } from '@shared/tui.module';
import { CoreHttpService } from './services/core-http.service';
import { NavigationService } from './services/navigation.service';

@NgModule({
  imports: [
    HttpClientModule,
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
