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
import { ThemeService } from './services/theme.service';
import { AvatarModule } from 'primeng/avatar';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { HomeService } from './services/home.service';

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
    ToastModule,
    ButtonModule,
    AvatarModule
  ],
  providers: [
    DifyFacade,
    DifySignalrEventsService,
    ThemeService,
    HomeService,
    MessageService
  ]
})
export class HomeModule { }
