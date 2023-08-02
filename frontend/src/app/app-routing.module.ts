import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/auth/guards/auth.guard';

const routes: Routes = [
  {
    path: 'start',
    loadChildren: async () => (await import('./modules/welcome/welcome.module')).WelcomeModule
  },
  {
    path: 'home',
    canActivate: [AuthGuard],
    loadChildren: async () => (await import('./modules/home/home.module')).HomeModule
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'home',
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
