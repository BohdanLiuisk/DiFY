import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  HostBinding,
  Input,
  OnDestroy,
  OnInit
} from '@angular/core';
import { MenuItem } from '@shared/modules/sidebar-menu/sidebar-menu.types';
import { BehaviorSubject, distinctUntilChanged, filter, Subject, takeUntil } from 'rxjs';
import { MenuRoleService } from '@shared/modules/sidebar-menu/services/menu-role.service';
import { MenuSearchService } from '@shared/modules/sidebar-menu/services/menu-search.service';
import { NavigationEnd, Router } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'li[df-menu-item][menuItem]',
  animations: [
    trigger('rotate', [
      state('true', style({ transform: 'rotate(-90deg)' })),
      transition('false <=> true', animate(`${300}ms ease-out`)),
    ])
  ],
  templateUrl: './menu-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MenuItemComponent implements OnInit, OnDestroy {
  @Input() public menuItem: MenuItem;
  @Input() public isRootNode: boolean = true;
  @Input() public level!: number;
  @Input() public disable: boolean = false;

  @HostBinding('class.df-menu-item--filtered') get filtered(): boolean {
    return this.isItemFiltered;
  }
  @HostBinding('class.df-menu-item--disabled') get disabled(): boolean {
    return this.isItemDisabled || this.disable;
  }

  private onDestroy$: Subject<void> = new Subject<void>();
  private isActive: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private isFiltered: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  public isActive$ = this.isActive.asObservable().pipe(distinctUntilChanged(), takeUntil(this.onDestroy$));
  public isFiltered$ = this.isFiltered.asObservable().pipe(distinctUntilChanged(), takeUntil(this.onDestroy$));

  public isItemFiltered = false;
  public isItemDisabled = false;

  constructor(
    private router: Router,
    public roleService: MenuRoleService,
    private searchService: MenuSearchService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }

  public ngOnInit(): void {
    this.routerItemActiveSubscription();
    this.emitItemActive();
    this.menuSearchSubscription();
    this.disabledItemSubscription();
  }

  public ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  public onNodeActive(event: boolean): void {
    this.isActive.next(event);
  }

  public onNodeFiltered(event: boolean): void {
    this.isItemFiltered = event;
    this.isFiltered.next(event);
  }

  private emitItemActive(): void {
    if (this.menuItem.route) {
      this.isActive.next(this.isActiveRoute(this.menuItem.route));
    }
  }

  private isActiveRoute(route: string): boolean {
    return this.router.isActive(route, this.isItemLinkExact());
  }

  private isItemLinkExact(): boolean {
    return this.menuItem.linkActiveExact === undefined ? true : this.menuItem.linkActiveExact;
  }

  private routerItemActiveSubscription(): void {
    this.router.events
      .pipe(
        filter((e): e is NavigationEnd => e instanceof NavigationEnd),
        takeUntil(this.onDestroy$)
      )
      .subscribe((e) => {
        this.emitItemActive();
      });
  }

  private menuSearchSubscription(): void {
    if (!this.menuItem.children) {
      this.searchService.search$.pipe(takeUntil(this.onDestroy$)).subscribe((search) => {
        this.isItemFiltered = this.searchService.filter(search, this.menuItem.label || this.menuItem.header);
        this.isFiltered.next(this.isItemFiltered);
        this.changeDetectorRef.markForCheck();
      });
    }
  }

  private disabledItemSubscription(): void {
    this.roleService
      .disableItem$(this.menuItem.roles)
      .pipe(takeUntil(this.onDestroy$))
      .subscribe((disabled) => (this.isItemDisabled = disabled));
  }
}
