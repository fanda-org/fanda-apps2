import { Component, Inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
// import { first } from 'rxjs/operators';

import { UnitService, AlertService } from '../../_services';
import { Unit } from '../../_models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-units',
  templateUrl: 'units.component.html'
})
export class UnitsComponent implements OnInit {
  isCollapsed: boolean = false;
  iconCollapse: string = 'icon-arrow-up';
  form: FormGroup;
  id: string;
  isAddMode: boolean;
  loading = false;
  submitted = false;
  units: Observable<Unit[]> = null;

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
      const x: Unit = this.unitService.get(this.id);
      // .pipe(first())
      // .subscribe((x) => {
      this.f.code.setValue(x.code);
      this.f.name.setValue(x.name);
      this.f.description.setValue(x.description);
      this.f.active.setValue(x.active);
      // });
    }

    this.units = this.unitService.getUnits;
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
      this.createUnit();
    } else {
      this.updateUnit();
    }
  }

  private createUnit(): void {
    const success = this.unitService.create(this.form.value);
    if (success) {
      this.alertService.success('Unit added successfully', { keepAfterRouteChange: true });
      this.router.navigate(['.', { relativeTo: this.route }]);
    } else {
      this.alertService.error('Error adding unit');
      this.loading = false;
    }
    // .pipe(first())
    // .subscribe(
    //   (data) => {
    //     this.alertService.success('Unit added successfully', { keepAfterRouteChange: true });
    //     this.router.navigate(['.', { relativeTo: this.route }]);
    //   },
    //   (error) => {
    //     this.alertService.error(error);
    //     this.loading = false;
    //   }
    // );
  }

  private updateUnit(): void {
    const success = this.unitService.update(this.id, this.form.value);
    if (success) {
      this.alertService.success('Update successful', { keepAfterRouteChange: true });
      this.router.navigate(['..', { relativeTo: this.route }]);
    } else {
      this.alertService.error('Error updating unit');
      this.loading = false;
    }
    // .pipe(first())
    // .subscribe(
    //   (data) => {
    //     this.alertService.success('Update successful', { keepAfterRouteChange: true });
    //     this.router.navigate(['..', { relativeTo: this.route }]);
    //   },
    //   (error) => {
    //     this.alertService.error(error);
    //     this.loading = false;
    //   }
    // );
  }

  deleteUnit(id: string): void {
    // const unit = this.units.find((x) => x.id === id);
    // unit.isDeleting = true;
    const success = this.unitService.delete(id);
    if (success) {
      // this.units.subscribe((u) => (this.units = u.filter((x) => x.id !== id)));
      this.units = this.unitService.getUnits;
    }
    // .pipe(first())
    // .subscribe(() => {
    //   this.units = this.units.filter((x) => x.id !== id);
    // });
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
