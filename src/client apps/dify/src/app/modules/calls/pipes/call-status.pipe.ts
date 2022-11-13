import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'callStatus' })
export class CallStatusPipe implements PipeTransform {
  transform(active: boolean): string {
    return active ? 'Active' : 'Ended';
  }
}
