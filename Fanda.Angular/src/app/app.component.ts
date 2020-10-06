import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd } from '@angular/router';
import { User } from './_models';
import { AuthenticationService } from './_services';

@Component({
    selector: 'app-root',
    template: '<router-outlet></router-outlet>'
})
export class AppComponent {
    isCollapsed = false;
    user: User;

    constructor(
        titleService: Title,
        router: Router,
        private authenticationService: AuthenticationService
    ) {
        router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                const title = this.getTitle(
                    router.routerState,
                    router.routerState.root
                ).join('-');
                console.log('title', title);
                titleService.setTitle(`Fanda: ${title}`);
            }
        });
        this.authenticationService.user.subscribe((x) => (this.user = x));
    }

    logout(): void {
        this.authenticationService.logout();
    }

    // collect that title data properties from all child routes
    // there might be a better way but this worked for me
    getTitle(state, parent): string[] {
        const data: string[] = [];
        if (parent && parent.snapshot.data && parent.snapshot.data.title) {
            data.push(parent.snapshot.data.title);
        }

        if (state && parent) {
            data.push(...this.getTitle(state, state.firstChild(parent)));
        }
        return data;
    }
}
