import { Injectable } from '@angular/core';
import { Http, Response, URLSearchParams } from '@angular/http';
import 'rxjs';
import { EmployeeScreening } from '../model/employee-screening';

@Injectable()
export class EmployeeScreeningService {
    private baseAddress: string;
    constructor(
        private http: Http
    ) {
        this.baseAddress = 'http://screeningautomationtoolapi.azurewebsites.net';
    }

    getEmployeeScreenings() {
        return this.http.get(`${this.baseAddress}/api/EmployeeScreening`, { withCredentials: true }).map(response => response.json().map(i => <EmployeeScreening>i));
    }

    checkScreenings(email: string) {
        return this.http.get(`${this.baseAddress}/api/EmployeeScreening/CheckScreenings/${email}`, {
            withCredentials: true
        });
    }

    passTest(activeTestId: number) {
        return this.http.put(`${this.baseAddress}/api/EmployeeScreening/PassTest/${activeTestId}`, {
            withCredentials: true
        });
    }
}