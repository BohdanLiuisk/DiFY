import {
  AfterViewInit,
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  EventEmitter, HostBinding,
  Input,
  OnDestroy,
  Output, QueryList,
  TrackByFunction, ViewChildren
} from '@angular/core';
import { MenuItem } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { MenuRoleService } from '@shared/modules/sidebar-menu/services/menu-role.service';
import { MenuItemComponent } from '@shared/modules/sidebar-menu/item/menu-item.component';
import { combineLatest, filter, Subject, takeUntil } from 'rxjs';
import { MenuNodeService } from '@shared/modules/sidebar-menu/services/menu-node.service';
import { animate, AUTO_STYLE, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'df-menu-node',
  animations: [
    trigger('openClose', [
      state('true', style({ height: AUTO_STYLE })),
      state('false', style({ height: 0 })),
      transition('false <=> true', animate(`${300}ms ease-in`)),
    ])
  ],
  templateUrl: './menu-node.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MenuNodeComponent implements AfterViewInit, OnDestroy {
  public isOpen: boolean = false;
  public isActiveChild: boolean = false;

  @Input() public level: number;
  @Input() public menuItem: MenuItem;
  @Input() public disable: boolean = false;

  @Output() public isActive: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() public isFiltered: EventEmitter<boolean> = new EventEmitter<boolean>();

  @HostBinding('class.df-menu-node--open') get open(): boolean {
    return this.isOpen;
  }

  @ViewChildren(MenuItemComponent) private menuItemComponents: QueryList<MenuItemComponent>;

  private onDestroy$: Subject<void> = new Subject<void>();

  public trackByItem : TrackByFunction<MenuItem> = (index, item) => item.id || index;

  constructor(
    public roleService: MenuRoleService,
    public nodeService: MenuNodeService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }

  public ngAfterViewInit(): void {
    this.openedNodeSubscription();
    this.activeItemsSubscription();
    this.filterItemsSubscription();
  }

  public ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  public onNodeToggleClick(): void {
    this.isOpen = !this.isOpen;
    this.nodeService.openedNode.next({ nodeComponent: this, nodeLevel: this.level });
    this.changeDetectorRef.markForCheck();
  }

  private openedNodeSubscription(): void {
    this.nodeService.openedNode
      .pipe(
        filter(() => this.isOpen),
        filter((node) => node.nodeComponent !== this),
        takeUntil(this.onDestroy$)
      )
      .subscribe((node) => {
        if (node.nodeLevel <= this.level) {
          this.isOpen = false;
          this.changeDetectorRef.markForCheck();
        }
      }
    );
  }

  private activeItemsSubscription(): void {
    const isChildrenItemsActive = this.menuItemComponents.map((item) => item.isActive$);
    if (isChildrenItemsActive && isChildrenItemsActive.length) {
      combineLatest(isChildrenItemsActive)
        .pipe(takeUntil(this.onDestroy$))
        .subscribe((itemsActiveState) => {
          this.isOpen = this.isActiveChild = itemsActiveState.includes(true);
          this.isActive.emit(this.isOpen);
        }
      );
    }
  }

  private filterItemsSubscription(): void {
    const isChildrenItemsFiltered = this.menuItemComponents.map((item) => item.isFiltered$);
    if (isChildrenItemsFiltered && isChildrenItemsFiltered.length) {
      combineLatest(isChildrenItemsFiltered)
        .pipe(takeUntil(this.onDestroy$))
        .subscribe((itemsFilteredState) => {
          const isItemsFiltered = itemsFilteredState.includes(false) === false;
          this.isFiltered.emit(isItemsFiltered);
        }
      );
    }
  }
}
