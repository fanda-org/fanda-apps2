import { Component, OnInit } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    Validators,
    FormArray,
    AbstractControl
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationService } from 'src/app/_services';
import { capitalize } from 'src/app/_utils';
import { Application, AppResource } from './../../_models';

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

    // data: Application = null;
    // editCache: { [key: string]: { edit: boolean; data: AppResource } } = {};
    // editCache: { edit: boolean; data: AppResource } = {
    //     edit: false,
    //     data: null
    // };
    isEditing = false;

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
            id: [null],
            code: ['', [Validators.required, Validators.maxLength(16)]],
            name: ['', [Validators.required, Validators.maxLength(50)]],
            description: ['', [Validators.maxLength(255)]],
            edition: ['', [Validators.required, Validators.maxLength(25)]],
            version: ['', [Validators.required, Validators.maxLength(16)]],
            active: [true],
            appResources: this.fb.array([])
        });
    }

    get formArray(): FormArray {
        return this.form.get('appResources') as FormArray;
    }

    // get appResources(): any {
    //     return this.form.controls.appResources['controls'];
    // }

    initItemRows(): FormGroup {
        return this.fb.group({
            id: [null],
            code: ['', [Validators.required]],
            name: ['', [Validators.required]],
            description: [''],
            resourceType: ['', [Validators.required]],
            creatable: [false],
            updatable: [false],
            deletable: [false],
            readable: [false],
            printable: [false],
            importable: [false],
            exportable: [false]
        });
    }

    addRow(): void {
        this.formArray.push(this.initItemRows());
    }

    startEdit(index: number): void {
        // this.editCache[index].edit = true;
        this.isEditing = true;
    }

    saveEdit(index: number): void {
        // const index = this.listOfData.findIndex((item) => item.id === id);
        // Object.assign(
        //     this.data.appResources[index],
        //     this.editCache[index].data
        // );
        // this.editCache[index].edit = false;
        this.isEditing = false;
    }

    cancelEdit(index: number): void {
        // const index = this.listOfData.findIndex((item) => item.id === id);
        // this.editCache[index] = {
        //     data: { ...this.data.appResources[index] },
        //     edit: false
        // };
        this.isEditing = false;
    }

    deleteRow(index: number): void {
        this.formArray.removeAt(index);
    }

    // updateEditCache(): void {
    //     this.data.appResources.forEach((item) => {
    //         this.editCache[item.id] = {
    //             edit: false,
    //             data: { ...item }
    //         };
    //     });
    // }

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
}
