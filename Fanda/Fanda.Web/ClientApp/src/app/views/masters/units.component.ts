import { Component, Inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { UnitService, AlertService } from '../../_services';

@Component({
  selector: 'app-units',
  templateUrl: 'units.component.html'
})
export class UnitsComponent implements OnInit {
  units = null;
  isCollapsed: boolean = false;
  iconCollapse: string = 'icon-arrow-up';
  form: FormGroup;
  id: string;
  isAddMode: boolean;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private unitService: UnitService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAddMode = !this.id;

    this.form = this.formBuilder.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: [''],
      active: ['']
    });

    if (!this.isAddMode) {
      this.unitService
        .getById(this.id)
        .pipe(first())
        .subscribe((x) => {
          this.f.code.setValue(x.code);
          this.f.name.setValue(x.name);
          this.f.description.setValue(x.description);
          this.f.active.setValue(x.active);
        });
    }

    this.unitService
      .getAll()
      .pipe(first())
      .subscribe((units) => (this.units = units));

    // this.http.get<Unit[]>(this.baseUrl + 'api/units').subscribe(result => {
    //    this.units = result;
    // }, error => console.error(error));
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
    if (this.isAddMode) {
      this.createUser();
    } else {
      this.updateUser();
    }
  }

  private createUser(): void {
    this.unitService
      .create(this.form.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.alertService.success('Unit added successfully', { keepAfterRouteChange: true });
          this.router.navigate(['.', { relativeTo: this.route }]);
        },
        (error) => {
          this.alertService.error(error);
          this.loading = false;
        }
      );
  }

  private updateUser(): void {
    this.unitService
      .update(this.id, this.form.value)
      .pipe(first())
      .subscribe(
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

  deleteUser(id: string): void {
    const unit = this.units.find((x) => x.id === id);
    unit.isDeleting = true;
    this.unitService
      .delete(id)
      .pipe(first())
      .subscribe(() => {
        this.units = this.units.filter((x) => x.id !== id);
      });
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
