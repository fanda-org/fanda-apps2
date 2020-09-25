import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd } from '@angular/router';

@Component({
    selector: 'app-root',
    template: '<router-outlet></router-outlet>',
})
export class AppComponent {
    isCollapsed = false;

    constructor(titleService: Title, router: Router) {
        router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                const title = this.getTitle(
                    router.routerState,
                    router.routerState.root
                ).join('-');
                console.log('title', title);
                titleService.setTitle(`${title} : Fanda`);
            }
        });
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
