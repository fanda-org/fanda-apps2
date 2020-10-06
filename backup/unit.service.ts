import { ApiResponse } from './../_models/api-response';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
// import { map, first } from 'rxjs/operators';
// import { List } from 'immutable';

import { environment } from '../../environments/environment';
import { Unit } from '../_models';

@Injectable({ providedIn: 'root' })
export class UnitService {
    private baseUrl = `${environment.apiUrl}/units`;
    private units: BehaviorSubject<Unit[]>;

    constructor(private router: Router, private http: HttpClient) {
        // this.loadInitialData();
    }

    get getUnits(): Observable<Unit[]> {
        return this.units.asObservable();
    }

    loadInitialData(): void {
        this.http.get<ApiResponse>(`${this.baseUrl}`).subscribe(
            (res) => {
                this.units = new BehaviorSubject<Unit[]>(res.data);
            },
            (err) => console.log('Error retrieving units')
        );
    }

    get(id: string): Unit {
        return this.units.getValue().find((unit: Unit) => unit.id === id);
    }

    create(unit: Unit): boolean {
        let success;
        this.http.post<ApiResponse>(`${this.baseUrl}`, unit).subscribe(
            (res) => {
                this.units.next(this.units.getValue().push(res.data));
                success = true;
            },
            (err) => {
                console.log('Error adding unit');
                success = false;
            }
        );
        return success;
    }

    update(id: string, updated: Unit): boolean {
        let success;
        this.http.put<ApiResponse>(`${this.baseUrl}/${id}`, updated).subscribe(
            (res) => {
                const units = this.units.getValue();
                const index = units.findIndex((u: Unit) => u.id === id);
                // const unit: Unit = units.get(index);
                this.units.next(units.set(index, updated));
                success = true;
            },
            (err) => {
                console.log('Error updating unit');
                success = false;
            }
        );
        return success;
    }

    delete(id: string): boolean {
        let success;
        this.http.delete(`${this.baseUrl}/${id}`).subscribe(
            (res) => {
                const units: List<Unit> = this.units.getValue();
                const index = units.findIndex((todo) => todo.id === id);
                this.units.next(units.delete(index));
                success = true;
            },
            (err) => {
                console.log('Error deleting unit');
                success = false;
            }
        );
        return success;
    }
}
