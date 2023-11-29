import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TuiModule } from '@shared/tui.module';
import { FeedRoutingModule } from './feed-routing.module';
import { FeedComponent } from './components/feed/feed.component';
import { ButtonModule } from 'primeng/button';

@NgModule({
  declarations: [
    FeedComponent
  ],
  imports: [
    FeedRoutingModule,
    SharedModule,
    TuiModule,
    ButtonModule
  ]
})
export class FeedModule { }
