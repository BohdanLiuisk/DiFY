import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
const routes: Routes =  [
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: 'social',
        loadChildren: async () => (await import('../social/social.module')).SocialModule
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
