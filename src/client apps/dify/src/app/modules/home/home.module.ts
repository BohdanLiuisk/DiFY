import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { HomeHeaderComponent } from './home-header/home-header.component';
import { SharedModule } from '@shared/shared.module';
import { HomeNavigationComponent } from './home-navigation/home-navigation.component';

@NgModule({
  declarations: [
    HomeComponent,
    HomeHeaderComponent,
    HomeNavigationComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SharedModule
  ]
})
export class HomeModule { }
