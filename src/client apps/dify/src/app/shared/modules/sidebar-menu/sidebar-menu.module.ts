import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarMenuComponent } from './sidebar/sidebar-menu.component';
import { MenuItemComponent } from './item/menu-item.component';
import { MenuAnchorComponent } from './anchor/menu-anchor.component';
import { MenuNodeComponent } from './node/menu-node.component';

@NgModule({
  declarations: [SidebarMenuComponent, MenuItemComponent, MenuAnchorComponent, MenuNodeComponent],
  imports: [RouterModule, CommonModule],
  exports: [SidebarMenuComponent]
})
export class SidebarMenuModule { }
