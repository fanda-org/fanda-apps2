import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
})
export class AppComponent {
  isCollapsed = false;

  constructor() {
    console.log('appComponent:constructor');
  }
}
