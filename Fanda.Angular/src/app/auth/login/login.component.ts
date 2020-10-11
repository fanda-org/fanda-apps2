import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
    FormBuilder,
    FormGroup,
    Validators,
    AbstractControl
} from '@angular/forms';
import { first } from 'rxjs/operators';
import { trigger, style, animate, transition } from '@angular/animations';

import { AuthenticationService } from '../../_services';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    animations: [
        trigger('fade', [
            transition('void => *', [
                style({ opacity: 0 }),
                animate(200, style({ opacity: 1 }))
            ])
        ])
    ]
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error = '';
    promptMessage: string = '';

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        if (this.authenticationService.userValue) {
            this.router.navigate(['/pages/']);
        }
    }

    ngOnInit(): void {
        this.loginForm = this.formBuilder.group(
            {
                userName: ['', Validators.required],
                password: ['', Validators.required],
                remember: [true]
            },
            { updateOn: 'submit' }
        );

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/pages/';
    }

    // convenience getter for easy access to form fields
    get f(): { [key: string]: AbstractControl } {
        return this.loginForm.controls;
    }

    submitForm(): void {
        // tslint:disable-next-line: forin
        // for (const i in this.loginForm.controls) {
        //     this.loginForm.controls[i].markAsDirty();
        //     this.loginForm.controls[i].updateValueAndValidity();
        // }

        // if (this.loginForm.valid) {
        //     this.router.navigate(['/pages/']);
        // }

        this.resetMessage();
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            this.validateAllFormFields(this.loginForm);
            return;
        }

        this.loading = true;
        this.authenticationService
            .login(this.f.userName.value, this.f.password.value)
            .pipe(first())
            .subscribe({
                next: (response) => {
                    this.router.navigate([this.returnUrl]);
                },
                error: (error) => {
                    this.error = error;
                    this.loading = false;
                    if (error === 'Bad Request') {
                        this.promptMessage =
                            'User name or password is incorrect';
                    } else {
                        this.promptMessage = 'Some unknown error occoured';
                    }
                }
            });
    }

    validateAllFormFields(formGroup: FormGroup) {
        for (const i in formGroup.controls) {
            formGroup.controls[i].markAsDirty();
            formGroup.controls[i].updateValueAndValidity();
        }
    }

    resetMessage(): void {
        this.promptMessage = '';
    }
}
