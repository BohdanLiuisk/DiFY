import { NgModule } from '@angular/core';
import { HomeComponent } from './components/home/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '@shared/shared.module';
import { SidebarMenuModule } from '@shared/modules/sidebar-menu/sidebar-menu.module';
import { TuiModule } from '@shared/tui.module';
import { IncomingCallNotificationComponent } from './components/incoming-call-notification/incoming-call-notification.component';
import { StoreModule } from '@ngrx/store';
import { difyFeature } from '@modules/home/store/dify.reducer';
import { EffectsModule } from '@ngrx/effects';
import { DifyEffects } from '@modules/home/store/dify.effects';
import { DifyFacade } from '@modules/home/store/dify.facade';
import { DifySignalrEventsService } from './services/dify-signalr.events';

@NgModule({
  declarations: [
    HomeComponent,
    IncomingCallNotificationComponent
  ],
  imports: [
    HomeRoutingModule,
    SharedModule,
    SidebarMenuModule,
    TuiModule,
    StoreModule.forFeature(difyFeature),
    EffectsModule.forFeature([DifyEffects])
  ],
  providers: [
    DifyFacade,
    DifySignalrEventsService
  ]
})
export class HomeModule { }
