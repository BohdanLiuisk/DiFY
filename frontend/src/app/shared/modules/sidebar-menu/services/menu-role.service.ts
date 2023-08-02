import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { Role } from '@shared/modules/sidebar-menu/sidebar-menu.types';

@Injectable()
export class MenuRoleService {
  private role$ = new BehaviorSubject<Role | undefined>(undefined);

  public set role(role: Role | undefined) {
    this.role$.next(role);
  }

  public showItem$(roles?: Role[]): Observable<boolean> {
    return this.role$.pipe(
      map((role) => isAuthorized(role, roles))
    );
  }

  public disableItem$(roles?: Role[]): Observable<boolean> {
    return this.role$.pipe(
      map((role) => !isAuthorized(role, roles))
    );
  }
}

function isAuthorized(userRole?: Role, itemRoles?: Role[]): boolean {
  if (!(typeof userRole === 'string') || !itemRoles || itemRoles.length === 0) {
    return true;
  }
  return (itemRoles as Role[]).includes(userRole as Role);
}
