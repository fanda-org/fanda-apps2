import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CollapseModule } from 'ngx-bootstrap/collapse';
// import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
// import { PopoverModule } from 'ngx-bootstrap/popover';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
// import { ProgressbarModule } from 'ngx-bootstrap/progressbar';

import { MastersRoutingModule } from './masters-routing.module';

import { ApplicationsComponent } from './applications.component';
import { ApplicationEditComponent } from './application-edit.component';
import { UnitsComponent } from './units.component';
import { UnitEditComponent } from './unit-edit.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CollapseModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule,
    MastersRoutingModule
  ],
  declarations: [ApplicationsComponent, ApplicationEditComponent, UnitsComponent, UnitEditComponent]
})
export class MastersModule {}
