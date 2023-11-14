import { Injectable } from '@angular/core';
import { DropdownItem, MultiselectDropdownState } from './dropdown-store.models';
import { ComponentStore } from '@ngrx/component-store';
import { Observable } from 'rxjs';

@Injectable()
export class MultiselectDropdownStore extends ComponentStore<MultiselectDropdownState> {
  private readonly items$: Observable<DropdownItem[]> = this.select(state => state.items);

  private readonly selectedItemIds$: Observable<number[]> = this.select(
    state => state.selectedItems.map(i => i.id));

  public readonly selectedItems$: Observable<DropdownItem[]> = this.select(state => state.selectedItems);

  public readonly selectionOpened$: Observable<boolean> = this.select(state => state.selectionOpened);

  public readonly itemsSelection$: Observable<DropdownItem[]> = this.select(
    this.items$,
    this.selectedItemIds$,
    (items, selectedItemIds) => items.filter(item => !selectedItemIds.includes(item.id))
  );

  public readonly selectedItemsCount$: Observable<number> = this.select(
    this.selectedItems$, (items) => items.length);

  public readonly selectionItemsCount$: Observable<number> = this.select(
    this.itemsSelection$, (selection) => selection.length);

  constructor() {
    super({ 
      items: [], 
      selectedItems: [],
      search: '',
      selectionOpened: false
    });
  }
  
  public readonly setSelectedItems = this.updater((state, selectedItems: DropdownItem[]) => ({
    ...state,
    selectedItems
  }));

  public readonly updateItems = this.updater((state, items: DropdownItem[]) => ({
    ...state,
    items
  }));

  public readonly selectItem = this.updater((state, item: DropdownItem) => ({
    ...state,
    selectedItems: [...state.selectedItems, item]
  }));

  public readonly removeSelected = this.updater((state, item: DropdownItem) => ({
    ...state,
    selectedItems: state.selectedItems.filter(i => i.id !== item.id)
  }));

  public readonly searchChanged = this.updater((state, search: string) => ({
    ...state,
    search
  }));

  public readonly hideSelection = this.updater((state) => ({
    ...state,
    selectionOpened: false
  }));

  public readonly openSelection = this.updater((state) => ({
    ...state,
    selectionOpened: true
  }));

  public readonly clearSelected = this.updater((state) => ({
    ...state,
    selectedItems: []
  }));

  public readonly toggleSelection = this.updater((state) => ({
    ...state,
    selectionOpened: !state.selectionOpened
  }));
}
