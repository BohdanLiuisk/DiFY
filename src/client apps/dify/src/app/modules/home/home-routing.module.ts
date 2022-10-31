import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
const routes: Routes =  [
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: 'calls',
        loadChildren: async () => (await import('../calls/calls.module')).CallsModule
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
