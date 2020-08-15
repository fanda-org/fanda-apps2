import { ApiResponse } from './../_models/api-response';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Application } from '../_models';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  private baseUrl = `${environment.authUrl}/applications`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.baseUrl}`);
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
}
