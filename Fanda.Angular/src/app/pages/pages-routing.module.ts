import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './welcome/welcome.component';
import { ApplicationEditComponent } from './application/application-edit.component';

const routes: Routes = [
  { path: '', component: WelcomeComponent },
  {
    path: 'application',
    component: ApplicationEditComponent,
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
