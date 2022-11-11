import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '@shared/shared.module';
import { MaterialModule } from '@shared/material.module';
import { SidebarMenuModule } from '@shared/modules/sidebar-menu/sidebar-menu.module';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    HomeRoutingModule,
    SharedModule,
    MaterialModule,
    SidebarMenuModule
  ]
})
export class HomeModule { }
