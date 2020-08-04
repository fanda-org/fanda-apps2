import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
// import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Unit } from '../_models';

@Injectable({ providedIn: 'root' })
export class UnitService {
  private baseUrl = `${environment.apiUrl}/units`;
  // private unitSubject: BehaviorSubject<Unit>;
  // public unit: Observable<Unit>;

  constructor(private router: Router, private http: HttpClient) {
    // this.unitSubject = new BehaviorSubject<Unit>(JSON.parse(localStorage.getItem('user')));
    // this.unit = this.unitSubject.asObservable();
  }

  // public get unitValue(): Unit {
  //     return this.unitSubject.value;
  // }

  getAll() {
    return this.http.get<Unit[]>(`${this.baseUrl}`);
  }

  getById(id: string) {
    return this.http.get<Unit>(`${this.baseUrl}/${id}`);
  }

  create(unit: Unit) {
    return this.http.post(`${this.baseUrl}`, unit);
  }

  update(id: string, unit: Unit) {
    return this.http.put(`${this.baseUrl}/${id}`, unit);
  }

  delete(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
