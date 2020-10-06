import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
    FormBuilder,
    FormGroup,
    Validators,
    AbstractControl
} from '@angular/forms';
import { first } from 'rxjs/operators';

import { AuthenticationService } from '../../_services';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error = '';

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
        this.loginForm = this.formBuilder.group({
            userName: ['', Validators.required],
            password: ['', Validators.required],
            remember: [true]
        });

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
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }

        this.loading = true;
        this.authenticationService
            .login(this.f.userName.value, this.f.password.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.router.navigate([this.returnUrl]);
                },
                error: (error) => {
                    this.error = error;
                    this.loading = false;
                }
            });
    }
}
