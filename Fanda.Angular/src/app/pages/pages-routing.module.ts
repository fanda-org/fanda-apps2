import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './welcome/welcome.component';
import { ApplicationsComponent } from './application/applications.component';
import { ApplicationEditComponent } from './application/application-edit.component';

const routes: Routes = [
    { path: '', component: WelcomeComponent },
    {
        path: 'applications',
        component: ApplicationsComponent,
    },
    {
        path: 'applications/:mode/:id',
        component: ApplicationEditComponent,
    },
    {
        path: 'applications/:mode',
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
