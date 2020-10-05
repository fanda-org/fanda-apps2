import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// import { NgZorroAntdModule } from 'ng-zorro-antd';
// import { NzGridModule } from 'ng-zorro-antd/grid';
// import { NzButtonModule } from 'ng-zorro-antd/button';
// import { NzFormModule } from 'ng-zorro-antd/form';
import { IconsProviderModule } from '../icons-provider.module';
import { NgZorroAntdModule } from '../ng-zorro-antd.module';

import { PagesRoutingModule } from './pages-routing.module';
import { WelcomeComponent } from './welcome/welcome.component';
import { ApplicationsComponent } from './application/applications.component';
import { ApplicationEditComponent } from './application/application-edit.component';
import { TenantsComponent } from './tenant/tenants.component';
import { UsersComponent } from './users/users.component';
import { RolesComponent } from './roles/roles.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        IconsProviderModule,
        NgZorroAntdModule,
        // CommonModule,
        // NzGridModule,
        // NzButtonModule,
        // NzFormModule,
        PagesRoutingModule
    ],
    declarations: [
        WelcomeComponent,
        ApplicationEditComponent,
        ApplicationsComponent,
        TenantsComponent,
        UsersComponent,
        RolesComponent
    ]
})
export class PagesModule {
    constructor() {
        console.log('pagesModule:constructor');
    }
}
