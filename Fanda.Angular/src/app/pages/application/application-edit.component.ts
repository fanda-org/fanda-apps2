import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationService } from 'src/app/_services';
import { capitalize } from 'src/app/_utils';

@Component({
    selector: 'app-application',
    templateUrl: './application-edit.component.html',
    styleUrls: ['./application-edit.component.css']
})
export class ApplicationEditComponent implements OnInit {
    form!: FormGroup;
    mode: string;
    id: string;
    loading = false;
    submitted = false;

    submitForm(): void {
        this.submitted = true;
        this.loading = false;
        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        // tslint:disable-next-line: forin
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
            this.form.controls[i].updateValueAndValidity();
        }
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
            code: [null, [Validators.required, Validators.maxLength(16)]],
            name: [null, [Validators.required, Validators.maxLength(50)]],
            description: [null, [Validators.maxLength(255)]],
            edition: [null, [Validators.required, Validators.maxLength(25)]],
            version: [null, [Validators.required, Validators.maxLength(16)]],
            active: [true]
        });
    }
}
