import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './welcome/welcome.component';
import { ApplicationComponent } from './application/application.component';

const routes: Routes = [
  { path: '', component: WelcomeComponent },
  {
    path: 'application',
    component: ApplicationComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
  constructor() {
    console.log('pagesRouting:constructor');
  }
}
