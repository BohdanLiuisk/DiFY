import { ChangeDetectionStrategy, Component, EventEmitter, HostBinding, Input, Output, ViewChild } from '@angular/core';
import { MenuItem } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { RouterLinkActive } from '@angular/router';
import { AnchorService } from '@shared/modules/sidebar-menu/services/anchor.service';

@Component({
  selector: 'df-menu-anchor',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './menu-anchor.component.html'
})
export class MenuAnchorComponent {
  @Input() public menuItem!: MenuItem;
  @Input() public isActive?: boolean;
  @Input() public disable: boolean = false;

  @Output() public anchorClicked: EventEmitter<void> = new EventEmitter<void>();

  @ViewChild('rla') private routerLinActive?: RouterLinkActive;

  @HostBinding("class.df-menu-anchor--active") get active(): boolean {
    return this.isActive || (!!this.routerLinActive?.isActive && !this.disable);
  }

  constructor(public anchorService: AnchorService) { }
}
