import { Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot
} from '@angular/router';

import { AuthenticationService } from '../_services';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        const user = this.authenticationService.userValue;
        if (user) return true;
        return this.redirectToLoginPage(state);
    }

    private redirectToLoginPage(state: RouterStateSnapshot) {
        this.router.navigate(['/auth/login'], {
            queryParams: { returnUrl: state.url }
        });
        return false;
    }
}
