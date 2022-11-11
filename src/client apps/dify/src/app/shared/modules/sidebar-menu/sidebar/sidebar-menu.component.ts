import { ChangeDetectionStrategy, Component, Input, TrackByFunction } from '@angular/core';
import { MenuNodeService } from '@shared/modules/sidebar-menu/services/menu-node.service';
import { MenuRoleService } from '../services/menu-role.service';
import { MenuSearchService } from '@shared/modules/sidebar-menu/services/menu-search.service';
import { Menu, MenuItem, MenuModes, Role } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { AnchorService } from '@shared/modules/sidebar-menu/services/anchor.service';

@Component({
  selector: 'df-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.scss'],
  providers: [MenuNodeService, MenuRoleService, MenuSearchService, AnchorService],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarMenuComponent {
  public disableAnimations: boolean = true;
  public menu?: Menu;

  @Input('menu') set _menu(menu: Menu) {
    this.disableAnimations = true;
    this.menu = menu;
    setTimeout(() => {
      this.disableAnimations = false;
    });
  }
  @Input() set iconClasses(cssClasses: string) {
    this.anchorService.iconClasses = cssClasses;
  }
  @Input() set toggleIconClasses(cssClasses: string) {
    this.nodeService.toggleIconClasses = cssClasses;
  }
  @Input() set role(role: Role | undefined) {
    this.roleService.role = role;
  }
  @Input() set search(value: string | undefined) {
    this.searchService.search = value;
  }
  @Input() public mode: MenuModes = MenuModes.EXPANDED;

  public trackByItem : TrackByFunction<MenuItem> = (index, item) => item.id || index;

  constructor(
    private nodeService: MenuNodeService,
    private searchService: MenuSearchService,
    public roleService: MenuRoleService,
    private anchorService: AnchorService
  ) { }
}
