import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { fandaMenus } from './fanda-menu';
import { authMenus } from './auth-menu';
import { AuthenticationService } from './../_services';
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
    menus = authMenus; // fandaMenus;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        console.log('defaultLayout:constructor');
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
