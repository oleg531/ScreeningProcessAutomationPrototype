import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs';
import { EmployeeScreeningTest } from '../model/employee-screening-test';

export class EmployeeScreeningService {
    constructor(
        private http: Http
    ) {
    }

    getEmployeeScreenings() {
        return this.http.get('').map(response => <EmployeeScreeningTest>response.json());
    }
}