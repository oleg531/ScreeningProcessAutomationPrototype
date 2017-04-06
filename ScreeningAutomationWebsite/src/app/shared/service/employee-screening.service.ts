import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs';
import { EmployeeScreening } from '../model/employee-screening';

@Injectable()
export class EmployeeScreeningService {
    constructor(
        private http: Http
    ) {
    }

    getEmployeeScreenings() {
        return this.http.get('http://localhost:3500/api/EmployeeScreening', { withCredentials: true }).map(response => response.json().map(i => <EmployeeScreening>i));
    }
}