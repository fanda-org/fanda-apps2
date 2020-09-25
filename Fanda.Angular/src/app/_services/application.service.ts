import { ApiResponse } from './../_models/api-response';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Application } from '../_models';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
    private baseUrl = `${environment.authUrl}/applications`;

    constructor(private http: HttpClient) {}

    getAll(
        page: number,
        pageSize: number,
        sort: string | null,
        filters: Array<{ key: string; value: string[] }>
    ): Observable<ApiResponse> {
        const filterCondition = this.getFilterCondition(filters);
        console.log(filterCondition);
        const params = new HttpParams()
            .append('page', `${page}`)
            .append('pageSize', `${pageSize}`)
            .append('sort', `${sort}`)
            .append('filter', filterCondition);

        return this.http.get<ApiResponse>(`${this.baseUrl}`, { params });
    }

    getById(id: string): Observable<ApiResponse> {
        return this.http.get<ApiResponse>(`${this.baseUrl}/${id}`);
    }

    create(application: Application): Observable<ApiResponse> {
        return this.http.post<ApiResponse>(`${this.baseUrl}`, application);
    }

    update(id: string, updated: Application): Observable<ApiResponse> {
        return this.http.put<ApiResponse>(`${this.baseUrl}/${id}`, updated);
    }

    delete(id: string): Observable<ApiResponse> {
        return this.http.delete<ApiResponse>(`${this.baseUrl}/${id}`);
    }

    getFilterCondition(
        filters: Array<{ key: string; value: string[] }>
    ): string {
        console.log('filters', filters);
        let filterCondition = '';
        if (filters) {
            filters.forEach((filter) => {
                if (filter.value) {
                    filterCondition += ' (';
                    filter.value.forEach((value) => {
                        filterCondition += `${filter.key}==${value} or `;
                    });
                    filterCondition =
                        filterCondition.substring(
                            0,
                            filterCondition.length - 4
                        ) + ')';
                }
                filterCondition += ' and ';
            });
            filterCondition = filterCondition.substring(
                0,
                filterCondition.length - 5
            );
            return filterCondition;
        }
    }
}
