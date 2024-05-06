import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseComponent } from '@core/components/base.component';
import { GUID } from '@shared/custom-types';
import { 
  Call, CallDirection, CallParticipantStatus 
} from '../../models/call-history.models';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroupModule } from 'primeng/avatargroup';
import { RouterModule } from '@angular/router';
import { TooltipModule } from 'primeng/tooltip';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { CallHistoryStore } from '../../store/history/history.store';
import { Observable } from 'rxjs';
import { SkeletonModule } from 'primeng/skeleton';
import { DifyFacade } from '@modules/home/store/dify.facade';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [
    CommonModule, RouterModule, ButtonModule, DividerModule, SkeletonModule,
    AvatarModule, AvatarGroupModule, TooltipModule, OverlayPanelModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss',
  providers: [CallHistoryStore]
})
export class HistoryComponent extends BaseComponent implements OnInit {
  public readonly maxParticipantsDisplay: number = 2;
  public readonly callHistory$: Observable<Call[]> = this.store.callsHistory$.pipe(
    this.untilThis
  );
  public readonly loading$: Observable<boolean> = this.store.loading$.pipe(
    this.untilThis
  );
  
  constructor(
    public readonly store: CallHistoryStore,
    private readonly difyFacade: DifyFacade) {
    super();
  }

  public ngOnInit(): void {
    this.store.loadHistory();
  }

  public joinCall(callId: GUID): void {
    this.difyFacade.joinCall(callId);
  }

  public getCallDirectionColor(call: Call): string {
    const direction = call.direction;
    const status = call.status;
    let color = '';
    if(direction === CallDirection.Incoming && status === CallParticipantStatus.Declined) {
      color = 'text-red-500';
    }
    else if(direction === CallDirection.Outgoing && status === CallParticipantStatus.Declined) {
      color = 'text-red-500';
    }
    return color;
  }
  
  public getCallDirectionIcon(call: Call): string {
    switch (call.direction) {
      case CallDirection.Incoming:
        return 'pi-arrow-down-left';
      case CallDirection.Outgoing:
        return 'pi-arrow-up-right';
    }
  }
  
  public getCallDirection(call: Call): string {
    return CallDirection[call.direction];
  }
}
