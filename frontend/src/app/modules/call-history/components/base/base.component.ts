import { Component } from '@angular/core';
import { BaseComponent } from '@core/components/base.component';
import { BaseCallHistoryStore } from '../../store/base/base.store';
import { 
  CallHistorySection, CreateNewCallConfig, callsSections 
} from '../../models/call-history.models';
import { Observable, filter, switchMap, tap } from 'rxjs';
import { NewCallFormComponent } from '../new-call-form/new-call-form.component';
import { Dialog } from '@angular/cdk/dialog';
import { CallHistoryService } from '@modules/call-history/services/call-history.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-call-history',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class CallHistoryComponent extends BaseComponent {
  private readonly sections: CallHistorySection[] = [
    {
      code: callsSections.history,
      caption: 'History',
      route: './',
      routeExact: true,
      icon: 'pi-book',
      loading: false,
      enabled: true,
      color: ''
    },
    {
      code: callsSections.callFriends,
      caption: 'Call friends',
      route: './call-friends',
      routeExact: true,
      icon: 'pi-users',
      loading: false,
      enabled: true,
      color: ''
    },
    {
      code: callsSections.active,
      caption: 'Active',
      route: '',
      routeExact: true,
      icon: 'pi-microphone',
      loading: false,
      enabled: false,
      color: 'text-green-400'
    },
    {
      code: callsSections.missed,
      caption: 'Missed',
      route: '',
      routeExact: true,
      icon: 'pi-arrow-down-left',
      loading: false,
      enabled: false,
      color: 'text-red-500'
    },
    {
      code: callsSections.sceduled,
      caption: 'Sceduled',
      route: '',
      routeExact: true,
      icon: 'pi-calendar',
      loading: false,
      enabled: false,
      color: ''
    }
  ];

  public readonly sections$: Observable<CallHistorySection[]> = this.store.sections$.pipe(
    this.untilThis
  )

  constructor(
    public readonly store: BaseCallHistoryStore,
    private readonly callHistoryService: CallHistoryService,
    private router: Router,
    public modalService: Dialog) {
    super();
    this.store.setSections(this.sections);
  }

  public createNewCall(): void {
    const dialogRef = this.modalService.open<CreateNewCallConfig>(NewCallFormComponent, {
      width: '40vw',
      disableClose: true,
      autoFocus: false
    });
    dialogRef.closed.pipe(
      this.untilThis,
      filter(result => Boolean(result)),
      switchMap(({ name, participantIds }) => 
        this.callHistoryService.createNew({ name, participantIds })
      ),
      tap(({ callId }) => 
        this.router.navigate([`home/call/${callId}`])
      )
    ).subscribe();
  }
}
