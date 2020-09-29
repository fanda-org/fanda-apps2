import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationService } from 'src/app/_services';
import { capitalize } from 'src/app/_utils';

@Component({
    selector: 'app-application',
    templateUrl: './application-edit.component.html',
    styleUrls: ['./application-edit.component.css'],
})
export class ApplicationEditComponent implements OnInit {
    form!: FormGroup;
    mode: string;
    id: string;
    loading = false;
    submitted = false;

    submitForm(): void {
        // tslint:disable-next-line: forin
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
            this.form.controls[i].updateValueAndValidity();
        }
    }

    get isHorizontal(): boolean {
        return this.form.controls.formLayout?.value === 'horizontal';
    }

    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private applicationService: ApplicationService
    ) {}

    ngOnInit(): void {
        this.mode = capitalize(this.route.snapshot.params.mode);
        this.id = this.route.snapshot.params.id;
        this.form = this.fb.group({
            formLayout: ['horizontal'],
            fieldA: [null, [Validators.required]],
            filedB: [null, [Validators.required]],
        });
    }
}
