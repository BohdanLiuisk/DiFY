export interface DropdownItem {
	id: number;
	name: string;
}

export interface MultiselectDropdownState {
  items: DropdownItem[];
	selectedItems: DropdownItem[];
	search: string;
	selectionOpened: boolean;
}
