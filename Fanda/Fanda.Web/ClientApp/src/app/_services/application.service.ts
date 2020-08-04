import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Application, ApiResponse } from '../_models';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  private baseUrl = `${environment.authUrl}/applications`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.baseUrl}`);
  }

  getById(id: string): any {
    return this.http.get<Application>(`${this.baseUrl}/${id}`);
  }

  create(app: Application): any {
    return this.http.post(`${this.baseUrl}`, app);
  }

  update(id: string, app: Application): any {
    return this.http.put(`${this.baseUrl}/${id}`, app);
  }

  delete(id: string): any {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
