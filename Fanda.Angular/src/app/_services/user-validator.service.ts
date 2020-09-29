import { Injectable } from '@angular/core';
import { ValidatorFn, AbstractControl } from '@angular/forms';
import { FormGroup } from '@angular/forms';

@Injectable({
    providedIn: 'root',
})
export class UserValidatorService {
    patternValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            if (!control.value) {
                return null;
            }
            const regex = new RegExp(
                '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$'
            );
            const valid = regex.test(control.value);
            return valid ? null : { invalidPassword: true };
        };
    }

    matchPassword(password: string, confirmPassword: string): ValidatorFn {
        return (formGroup: FormGroup) => {
            const passwordControl = formGroup.controls[password];
            const confirmPasswordControl = formGroup.controls[confirmPassword];

            if (!passwordControl || !confirmPasswordControl) {
                return null;
            }

            if (
                confirmPasswordControl.errors &&
                !confirmPasswordControl.errors.passwordMismatch
            ) {
                return null;
            }

            if (passwordControl.value !== confirmPasswordControl.value) {
                confirmPasswordControl.setErrors({ passwordMismatch: true });
            } else {
                confirmPasswordControl.setErrors(null);
            }
        };
    }

    userNameValidator(
        userControl: AbstractControl
    ): Promise<{ userNameNotAvailable: boolean }> {
        return new Promise((resolve) => {
            // setTimeout(() => {
            if (this.validateUserName(userControl.value)) {
                resolve({ userNameNotAvailable: true });
            } else {
                resolve(null);
            }
            // }, 1000);
        });
    }

    validateUserName(userName: string): boolean {
        const UserList = ['ankit', 'admin', 'user', 'superuser'];
        return UserList.indexOf(userName) > -1;
    }

    mustMatch(controlName: string, matchingControlName: string): ValidatorFn {
        return (formGroup: FormGroup) => {
            const control = formGroup.controls[controlName];
            const matchingControl = formGroup.controls[matchingControlName];

            if (matchingControl.errors && !matchingControl.errors.mustMatch) {
                // return if another validator has already found an error on the matchingControl
                return null;
            }

            // set error on matchingControl if validation fails
            if (control.value !== matchingControl.value) {
                matchingControl.setErrors({ mustMatch: true });
            } else {
                matchingControl.setErrors(null);
            }
        };
    }
}
