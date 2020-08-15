import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { ApplicationService, AlertService, HiddenDataService } from '../../_services';
import { Application } from '../../_models';

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html'
})
export class ApplicationsComponent implements OnInit {
  isCollapsed: boolean = false;
  iconCollapse: string = 'icon-arrow-up';
  loading = false;
  submitted = false;
  applications: Application[] = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private applicationService: ApplicationService,
    private alertService: AlertService,
    private hiddenService: HiddenDataService
  ) {}

  ngOnInit(): void {
    this.applicationService.getAll().subscribe((res) => {
      this.applications = res.data;
      this.alertService.success('Create successful', { keepAfterRouteChange: true });
    });
  }

  onClick(mode: string, id: string): void {
    console.log(mode, id);
    this.hiddenService.id = id;
    this.router.navigate([`/masters/applications/${mode}`]);
  }

  collapsed(event: any): void {
    // console.log(event);
  }

  expanded(event: any): void {
    // console.log(event);
  }

  toggleCollapse(): void {
    this.isCollapsed = !this.isCollapsed;
    this.iconCollapse = this.isCollapsed ? 'icon-arrow-down' : 'icon-arrow-up';
  }
}
