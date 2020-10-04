import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './welcome/welcome.component';
import { ApplicationsComponent } from './application/applications.component';
import { ApplicationEditComponent } from './application/application-edit.component';
import { TenantsComponent } from './tenant/tenants.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
    { path: '', component: WelcomeComponent, data: { title: 'Dashboard' } },
    {
        path: 'applications',
        component: ApplicationsComponent,
        data: { title: 'Applications' }
    },
    {
        path: 'applications/:mode/:id',
        component: ApplicationEditComponent,
        data: { title: `Application` }
    },
    {
        path: 'applications/:mode',
        component: ApplicationEditComponent,
        data: { title: 'Application' }
    },
    {
        path: 'tenants',
        component: TenantsComponent,
        data: { title: 'Tenants' }
    },
    {
        path: 'users',
        component: UsersComponent,
        data: { title: 'Users' }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PagesRoutingModule {
    constructor() {
        console.log('pagesRouting:constructor');
    }
}
