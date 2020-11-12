import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DefaultLayoutComponent } from './layouts/default-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout.component';
import { AuthGuard } from './_helpers';

const routes: Routes = [
    {
        path: 'auth',
        component: AuthLayoutComponent,
        loadChildren: () =>
            import('./auth/auth.module').then((m) => m.AuthModule)
    },
    {
        path: 'pages',
        component: DefaultLayoutComponent,
        loadChildren: () =>
            import('./pages/pages.module').then((m) => m.PagesModule),
        canActivate: [AuthGuard]
    },
    {
        path: '',
        redirectTo: '/auth/login',
        pathMatch: 'full'
    },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
    exports: [RouterModule]
})
export class AppRoutingModule {
    constructor() {
        console.log('appRouting:constructor');
    }
}
