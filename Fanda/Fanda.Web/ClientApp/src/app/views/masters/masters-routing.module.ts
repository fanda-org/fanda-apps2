import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UnitsComponent } from './units.component';
import { UnitEditComponent } from './unit-edit.component';
import { ApplicationsComponent } from './applications.component';
import { ApplicationEditComponent } from './application-edit.component';

const routes: Routes = [
  {
    path: 'units',
    component: UnitsComponent,
    data: { title: 'Units' }
  },
  {
    path: 'applications',
    component: ApplicationsComponent,
    data: { title: 'Applications' }
  },
  {
    path: 'applications/:mode/:id',
    component: ApplicationEditComponent
  },
  {
    path: 'applications/:mode',
    component: ApplicationEditComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MastersRoutingModule {}
