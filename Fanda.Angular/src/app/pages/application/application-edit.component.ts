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

    i = 0;
    editId: string | null = null;
    listOfData: ItemData[] = [];

    submitForm(): void {
        console.log('Form submitted');
        this.submitted = true;
        this.loading = false;
        // stop here if form is invalid
        // if (this.form.invalid) {
        //     return;
        // }

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
            code: ['', [Validators.required, Validators.maxLength(16)]],
            name: ['', [Validators.required, Validators.maxLength(50)]],
            description: ['', [Validators.maxLength(255)]],
            edition: ['', [Validators.required, Validators.maxLength(25)]],
            version: ['', [Validators.required, Validators.maxLength(16)]],
            active: [true]
        });

        this.addRow();
        this.addRow();
    }

    startEdit(id: string): void {
        this.editId = id;
    }

    stopEdit(): void {
        this.editId = null;
    }

    addRow(): void {
        this.listOfData = [
            ...this.listOfData,
            {
                id: `${this.i}`,
                name: `Edward King ${this.i}`,
                age: '32',
                address: `London, Park Lane no. ${this.i}`
            }
        ];
        this.i++;
    }

    deleteRow(id: string): void {
        this.listOfData = this.listOfData.filter((d) => d.id !== id);
    }
}

interface ItemData {
    id: string;
    name: string;
    age: string;
    address: string;
}
