import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WelcomeComponent } from './welcome.component';

const routes: Routes = [
  {
    path: '',
    component: WelcomeComponent,
    children: [
      {
        path: 'sign-in',
        loadChildren: () => import('../sign-in/sign-in.module')
          .then(m => m.SignInModule)
      },
      {
        path: 'sign-up',
        loadChildren: () => import('../sign-up/sign-up.module')
          .then(m => m.SignUpModule)
      }
    ]
  }
];

@NgModule({
  exports: [ RouterModule ],
  imports: [ RouterModule.forChild(routes) ],
})
export class WelcomeRoutingModule { }
