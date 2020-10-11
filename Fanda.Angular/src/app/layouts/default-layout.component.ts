import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { fandaMenus } from './fanda-menu';
import { authMenus } from './auth-menu';
import { AuthenticationService, TenantService } from './../_services';
import { Tenant, Menu } from '../_models';

@Component({
    selector: 'app-default-layout',
    templateUrl: './default-layout.component.html',
    styleUrls: ['./default-layout.component.css']
})
export class DefaultLayoutComponent {
    // title = 'Fanda';
    isCollapsed = false;
    mode = false;
    dark = false;
    openMenu: { [name: string]: boolean } = {
        invoice: true,
        transaction: false,
        inventory: false,
        contacts: false,
        accounts: false
    };
    menus: Menu[] = authMenus; // fandaMenus;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        tenantService: TenantService
    ) {
        console.log('defaultLayout:constructor');

        tenantService
            .getById(authenticationService.userValue.tenantId)
            .subscribe(
                (res) => {
                    const tenant = res.data as Tenant;
                    if (tenant.code === 'FANDA') {
                        this.menus = authMenus;
                    } else {
                        this.menus = fandaMenus;
                    }
                },
                (err) => {
                    console.log('Error', err);
                }
            );
    }

    onClick(path: string[]): void {
        this.router.navigate(path);
    }

    openHandler(value: string): void {
        // for (const key in this.openMenu) {
        //   if (key !== value) {
        //     this.openMenu[key] = false;
        //   }
        // }
        for (const menu of this.menus) {
            menu.open = true;
            if (menu.title !== value) {
                menu.open = false;
            }
        }
    }

    logout(): void {
        this.authenticationService.logout();
    }
}
