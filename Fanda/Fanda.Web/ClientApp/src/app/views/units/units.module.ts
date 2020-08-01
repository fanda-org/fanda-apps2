import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CollapseModule } from 'ngx-bootstrap/collapse';
//import { TabsModule } from 'ngx-bootstrap/tabs';
//import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//import { PopoverModule } from 'ngx-bootstrap/popover';
//import { TooltipModule } from 'ngx-bootstrap/tooltip';
//import { ProgressbarModule } from 'ngx-bootstrap/progressbar';

import { UnitsComponent } from './units.component';
import { UnitsRoutingModule } from './units-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        CollapseModule.forRoot(),
        UnitsRoutingModule,
    ],
    declarations: [UnitsComponent]
})
export class UnitsModule { }
