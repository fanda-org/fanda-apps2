import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent,
        data: { title: 'Login' },
    },
    {
        path: 'register',
        component: RegisterComponent,
        data: { title: 'Register' },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AuthRoutingModule {
    constructor() {
        console.log('authRouting:constructor');
    }
}
