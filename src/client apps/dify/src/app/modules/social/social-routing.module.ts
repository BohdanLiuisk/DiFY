import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  {
    path: '',
    loadChildren: async () => (await import('../feed/feed.module')).FeedModule
  },
  {
    path: 'calls',
    loadChildren: async () => (await import('../calls/calls.module')).CallsModule
  }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class SocialRoutingModule { }
