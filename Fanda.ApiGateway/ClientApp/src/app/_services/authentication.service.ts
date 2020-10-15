import { ApiResponse } from './../_models/api-response';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { User } from '../_models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private userSubject: BehaviorSubject<User>;
    public user: Observable<User>;
    private refreshTokenTimeout: number;

    constructor(private router: Router, private http: HttpClient) {
        this.userSubject = new BehaviorSubject<User>(null);
        this.user = this.userSubject.asObservable();
    }

    public get userValue(): User {
        return this.userSubject.value;
    }

    login(userName: string, password: string): Observable<User> {
        return this.http
            .post<ApiResponse>(
                `${environment.authUrl}/auth/login`,
                { userName, password },
                { withCredentials: true }
            )
            .pipe(
                map((res) => {
                    console.log('User by login', res);
                    this.userSubject.next(res.data);
                    this.startRefreshTokenTimer();
                    return res.data;
                })
            );
    }

    logout(): void {
        this.http
            .post<ApiResponse>(
                `${environment.authUrl}/auth/revoke-token`,
                {},
                { withCredentials: true }
            )
            .subscribe();
        this.stopRefreshTokenTimer();
        this.userSubject.next(null);
        this.router.navigate(['/auth/login']);
    }

    refreshToken(): Observable<User> {
        return this.http
            .post<ApiResponse>(
                `${environment.authUrl}/auth/refresh-token`,
                {},
                { withCredentials: true }
            )
            .pipe(
                map((res) => {
                    console.log('User by token', res);
                    this.userSubject.next(res.data);
                    this.startRefreshTokenTimer();
                    return res.data;
                })
            );
    }

    // helper methods

    private startRefreshTokenTimer(): void {
        // parse json object from base64 encoded jwt token
        const jwtToken = JSON.parse(
            atob(this.userValue.jwtToken.split('.')[1])
        );

        // set a timeout to refresh the token a minute before it expires
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - 60 * 1000;
        this.refreshTokenTimeout = setTimeout(
            () => this.refreshToken().subscribe(),
            timeout
        );
    }

    private stopRefreshTokenTimer(): void {
        clearTimeout(this.refreshTokenTimeout);
    }
}
