import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Tenant, ApiResponse, FilterModel } from '../_models';

@Injectable({ providedIn: 'root' })
export class TenantService {
    private baseUrl = `${environment.authUrl}/tenants`;

    constructor(private http: HttpClient) {}

    getAll(
        page: number,
        pageSize: number,
        sort: string | null,
        filters: Array<FilterModel>,
        customFilters: FilterModel[]
    ): Observable<ApiResponse> {
        const filterCondition = this.getFilterCondition(filters);
        const customCondition = this.getCustomCondition(customFilters);
        let combinedCondition = '';
        if (filterCondition) {
            combinedCondition = filterCondition;
        }
        if (customCondition) {
            if (filterCondition) {
                combinedCondition += ' and ' + customCondition;
            } else {
                combinedCondition = customCondition;
            }
        }
        console.log('Combined Filters', combinedCondition);

        const params = new HttpParams()
            .append('pageIndex', `${page}`)
            .append('pageSize', `${pageSize}`)
            .append('sort', `${sort}`)
            .append('filter', combinedCondition);

        return this.http.get<ApiResponse>(`${this.baseUrl}`, { params });
    }

    getById(id: string): Observable<ApiResponse> {
        return this.http.get<ApiResponse>(`${this.baseUrl}/${id}`);
    }

    create(tenant: Tenant): Observable<ApiResponse> {
        return this.http.post<ApiResponse>(`${this.baseUrl}`, tenant);
    }

    update(id: string, updated: Tenant): Observable<ApiResponse> {
        return this.http.put<ApiResponse>(`${this.baseUrl}/${id}`, updated);
    }

    delete(id: string): Observable<ApiResponse> {
        return this.http.delete<ApiResponse>(`${this.baseUrl}/${id}`);
    }

    activate(id: string, active: boolean): Observable<ApiResponse> {
        console.log('url', `${this.baseUrl}/activate/${id}`, {
            id,
            active
        });
        return this.http.patch<ApiResponse>(`${this.baseUrl}/activate/${id}`, {
            id,
            active
        });
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
            return `${key}.Contains("${value}")`; // , StringComparison.OrdinalIgnoreCase)`;
            // return `DynamicFunctions.Like(${key}, "%${value}%")`;
            // return `EF.Functions.Like(${key}, "%${value}%")`;
        }
    }

    getCustomCondition(filters: Array<FilterModel>): string {
        let filterCondition = '';
        if (filters) {
            filters.forEach((filter) => {
                if (filter && filter.value != null) {
                    filterCondition += this.getConditionByType(
                        filter.key,
                        filter.value
                    );
                    filterCondition += filterCondition ? ' or ' : '';
                }
            });
            filterCondition = filterCondition.substring(
                0,
                filterCondition.length - 4
            );
        }
        return filterCondition ? '(' + filterCondition + ')' : filterCondition;
    }
}
