import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UnitsComponent } from './units.component';
import { ApplicationComponent } from './application.component';

const routes: Routes = [
  {
    path: 'units',
    component: UnitsComponent,
    data: { title: 'Units' }
  },
  {
    path: 'applications',
    component: ApplicationComponent,
    data: { title: 'Applications' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MastersRoutingModule {}
