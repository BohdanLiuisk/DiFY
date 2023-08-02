import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AuthModule } from '@core/auth/auth.module';
import { AuthInterceptor } from '@core/auth/auth.interceptor';
import { BaseComponent } from './components/base.component';

@NgModule({
  imports: [
    HttpClientModule,
    SharedModule,
    AuthModule,
    StoreModule.forRoot({}, {}),
    EffectsModule.forRoot([])
  ],
  exports: [ ],
  declarations: [ BaseComponent ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class CoreModule { }
