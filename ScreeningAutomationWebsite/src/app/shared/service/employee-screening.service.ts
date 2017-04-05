import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs';
import { EmployeeScreening } from '../model/employee-screening';

export class EmployeeScreeningService {
    constructor(
        private http: Http
    ) {
    }

    getEmployeeScreenings() {
        return this.http.get('').map(response => <EmployeeScreening>response.json());
    }
}