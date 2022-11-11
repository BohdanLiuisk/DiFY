import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { MenuNodeComponent } from '@shared/modules/sidebar-menu/node/menu-node.component';

@Injectable()
export class MenuNodeService {
  public openedNode = new Subject<{ nodeComponent: MenuNodeComponent; nodeLevel: number }>();

  public toggleIconClasses?: string;
}
