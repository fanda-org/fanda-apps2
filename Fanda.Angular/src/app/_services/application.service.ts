import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Application, ApiResponse, FilterModel } from '../_models';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
    private baseUrl = `${environment.authUrl}/applications`;

    constructor(private http: HttpClient) {}

    getAll(
        page: number,
        pageSize: number,
        sort: string | null,
        filters: Array<FilterModel>
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

    getFilterCondition(filters: Array<FilterModel>): string {
        // console.log('filters', filters);
        let filterCondition = '';
        if (filters) {
            filters.forEach((filter) => {
                if (filter && filter.value !== null) {
                    // filterCondition += ' (';
                    if (Array.isArray(filter.value)) {
                        filter.value.forEach((value) => {
                            filterCondition +=
                                this.getConditionByType(filter.key, value) +
                                ' or ';
                        });
                        filterCondition = filterCondition.substring(
                            0,
                            filterCondition.length - 4
                        );
                    } else {
                        filterCondition += this.getConditionByType(
                            filter.key,
                            filter.value
                        );
                    }
                    // + ')';
                    filterCondition += filterCondition ? ' and ' : '';
                }
            });
            filterCondition = filterCondition.substring(
                0,
                filterCondition.length - 5
            );
            return filterCondition;
        }
    }

    getConditionByType(key: string, value: any): string {
        if (typeof value === 'boolean' || typeof value === 'number') {
            return `${key}==${value}`;
        } else if (typeof value === 'string') {
            return `${key}.Contains("${value}")`;
        }
    }
}
