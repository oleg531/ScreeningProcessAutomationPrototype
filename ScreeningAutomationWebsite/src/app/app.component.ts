import { Component, OnInit } from '@angular/core';
import { EmployeeScreeningService } from './shared/service/employee-screening.service';
import { EmployeeScreening } from './shared/model/employee-screening';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  private employeeScreenings: Array<EmployeeScreening>;
  constructor(
    private employeeScreeningService: EmployeeScreeningService
  ) {
    this.employeeScreenings = [];
  }

  ngOnInit() {
    this.employeeScreeningService.getEmployeeScreenings().subscribe(result => {
      this.employeeScreenings = result;
    }, error => console.error(error));
  }
}

