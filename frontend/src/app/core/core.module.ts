import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AuthModule } from '@core/auth/auth.module';
import { AuthInterceptor } from '@core/auth/auth.interceptor';
import { TuiModule } from '@shared/tui.module';

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
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class CoreModule { }
