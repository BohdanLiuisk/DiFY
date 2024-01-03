import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseComponent } from '@core/components/base.component';
import { CallHistoryFacade } from '@modules/call-history/store/call-history.facade';
import { GUID } from '@shared/custom-types';
import { Call, CallDirection } from '@modules/call-history/models/call-history.models';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroupModule } from 'primeng/avatargroup';
import { RouterModule } from '@angular/router';
import { TooltipModule } from 'primeng/tooltip';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [
    CommonModule, RouterModule, ButtonModule, DividerModule,
    AvatarModule, AvatarGroupModule, TooltipModule, OverlayPanelModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent extends BaseComponent implements OnInit {
  public readonly pageSizeOptions: number[] = [5, 10, 25, 100];
  public readonly maxParticipantsDisplay: number = 2;
  
  constructor(
    public callHistoryFacade: CallHistoryFacade) {
    super();
  }

  public ngOnInit(): void {
    this.callHistoryFacade.setPage(1);
  }

  public joinCall(callId: GUID): void {
    this.callHistoryFacade.joinCall(callId);
  }

  public getCallStatusTag(call: Call): string {
    return call.active ? 'Active': 'Ended';
  }

  public getCallDirectionColor(call: Call): string {
    switch (call.direction) {
      case CallDirection.Declined:
      case CallDirection.Missed:
      case CallDirection.Canceled:
        return 'text-red-500';
      case CallDirection.Incoming:
      case CallDirection.Outgoing:
        return 'text-green-500';
    }
  }
  
  public getCallDirectionIcon(call: Call): string {
    switch (call.direction) {
      case CallDirection.Declined:
      case CallDirection.Missed:
      case CallDirection.Incoming:
        return 'pi-arrow-down-left';
      case CallDirection.Canceled:
      case CallDirection.Outgoing:
        return 'pi-arrow-up-right';
    }
  }
  
  public getCallDirection(call: Call): string {
    return CallDirection[call.direction];
  }
}
