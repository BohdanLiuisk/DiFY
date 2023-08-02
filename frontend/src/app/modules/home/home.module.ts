import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '@shared/shared.module';
import { SidebarMenuModule } from '@shared/modules/sidebar-menu/sidebar-menu.module';
import { TuiModule } from '@shared/tui.module';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    HomeRoutingModule,
    SharedModule,
    SidebarMenuModule,
    TuiModule
  ]
})
export class HomeModule { }
