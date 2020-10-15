import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ApplicationService, AlertService, HiddenDataService } from '../../_services';
import { capitalize } from '../../_utils';

@Component({
  selector: 'app-application-edit',
  templateUrl: './application-edit.component.html'
})
export class ApplicationEditComponent implements OnInit {
  isCollapsed: boolean = false;
  iconCollapse: string = 'icon-arrow-up';
  form: FormGroup;
  mode: string;
  id: string;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private applicationService: ApplicationService,
    private alertService: AlertService,
    private hiddenService: HiddenDataService
  ) {}

  ngOnInit(): void {
    this.mode = capitalize(this.route.snapshot.params['mode']);
    this.id = this.hiddenService.id;

    this.form = this.formBuilder.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: [''],
      active: ['true']
    });

    if (this.id) {
      this.applicationService.getById(this.id).subscribe((x) => {
        this.f.code.setValue(x.data.code);
        this.f.name.setValue(x.data.name);
        this.f.description.setValue(x.data.description);
        this.f.active.setValue(x.data.active);
      });
    }
  }

  // convenience getter for easy access to form fields
  get f(): any {
    return this.form.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    if (!this.id) {
      this.createApplication();
    } else {
      this.updateApplication();
    }
  }

  private createApplication(): void {
    this.applicationService.create(this.form.value).subscribe(
      (data) => {
        this.alertService.success('Create successful', { keepAfterRouteChange: true });
        this.router.navigate(['.', { relativeTo: this.route }]);
      },
      (error) => {
        this.alertService.error(error);
        this.loading = false;
      }
    );
  }

  private updateApplication(): void {
    this.applicationService.update(this.id, this.form.value).subscribe(
      (data) => {
        this.alertService.success('Update successful', { keepAfterRouteChange: true });
        this.router.navigate(['..', { relativeTo: this.route }]);
      },
      (error) => {
        this.alertService.error(error);
        this.loading = false;
      }
    );
  }

  deleteApplication(id: string): void {
    // const app = this.applications.find((x) => x.id === id);
    // app.isDeleting = true;
    this.applicationService.delete(id).subscribe(
      () => {
        this.alertService.success('Delete successful', { keepAfterRouteChange: true });
        // this.applications = this.applications.filter((x) => x.id !== id);
      },
      (error) => {
        this.alertService.error(error);
        this.loading = false;
      }
    );
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
