type MenuItemId = number | string;

export type Role = string | number;

export enum MenuModes {
  EXPANDED = 'expanded',
  EXPANDABLE = 'expandable',
  COLLAPSED = 'mini',
}

export interface MenuItemBadge {
  label: string;
  classes: string;
}

export interface MenuItemBase {
  id?: MenuItemId;
  label: string;
  iconClass?: string;
  badges?: MenuItemBadge[];
  roles?: Role[];
}

export interface MenuItemLeafRoute extends MenuItemBase {
  route: string;
  linkActiveExact?: boolean;
}

export interface MenuItemLeafUrl extends MenuItemBase {
  url: string;
  target?: string;
}

export interface MenuItemHeader {
  id?: MenuItemId;
  header: string;
}

export interface MenuItemNode extends MenuItemBase {
  children: MenuItem[];
}

type Without<T, U> = { [P in Exclude<keyof T, keyof U>]?: never };

type XOR<T, U> = T | U extends object ? (Without<T, U> & U) | (Without<U, T> & T) : T | U;

export type MenuItem = XOR<MenuItemLeafRoute, XOR<MenuItemLeafUrl, XOR<MenuItemHeader, MenuItemNode>>>;

export type Menu = MenuItem[];
