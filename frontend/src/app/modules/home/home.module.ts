import { NgModule } from '@angular/core';
import { HomeComponent } from './components/home/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '@shared/shared.module';
import { 
  IncomingCallNotificationComponent 
} from './components/incoming-call-notification/incoming-call-notification.component';
import { StoreModule } from '@ngrx/store';
import { difyFeature } from '@modules/home/store/dify.reducer';
import { EffectsModule } from '@ngrx/effects';
import { DifyEffects } from '@modules/home/store/dify.effects';
import { DifyFacade } from '@modules/home/store/dify.facade';
import { DifySignalrEventsService } from './services/dify-signalr.events';
import { ButtonModule } from 'primeng/button';
import { ToggleButtonModule } from 'primeng/togglebutton';

@NgModule({
  declarations: [
    HomeComponent,
    IncomingCallNotificationComponent
  ],
  imports: [
    HomeRoutingModule,
    SharedModule,
    StoreModule.forFeature(difyFeature),
    EffectsModule.forFeature([DifyEffects]),
    ButtonModule,
    ToggleButtonModule
  ],
  providers: [
    DifyFacade,
    DifySignalrEventsService
  ]
})
export class HomeModule { }
