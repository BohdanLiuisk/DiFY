import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { FeedRoutingModule } from './feed-routing.module';
import { FeedComponent } from './feed.component';

@NgModule({
  declarations: [
    FeedComponent
  ],
  imports: [
    FeedRoutingModule,
    SharedModule,
    TuiModule
  ]
})
export class FeedModule { }
