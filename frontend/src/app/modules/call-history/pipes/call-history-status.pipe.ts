import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'callHistoryStatus' })
export class CallHistoryStatusPipe implements PipeTransform {
  transform(active: boolean): string {
    return active ? 'Active' : 'Ended';
  }
}
