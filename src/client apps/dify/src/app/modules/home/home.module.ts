import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { HomeHeaderComponent } from './home-header/home-header.component';
import { SharedModule } from '@shared/shared.module';
import { HomeNavigationComponent } from './home-navigation/home-navigation.component';
import { TuiModule } from '@shared/tui.module';

@NgModule({
  declarations: [
    HomeComponent,
    HomeHeaderComponent,
    HomeNavigationComponent
  ],
  imports: [
    HomeRoutingModule,
    SharedModule,
    TuiModule
  ]
})
export class HomeModule { }
