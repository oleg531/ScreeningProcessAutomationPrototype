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
  private commonEmail: string;
  private checkingProcess: boolean;
  private passingProcess: boolean;
  constructor(
    private employeeScreeningService: EmployeeScreeningService
  ) {
    this.employeeScreenings = [];
    this.checkingProcess = false;
    this.passingProcess = false;
  }

  ngOnInit() {
    this.loadGrid();
  }

  loadGrid() {
    this.employeeScreeningService.getEmployeeScreenings().subscribe(result => {
      this.employeeScreenings = result;
    }, error => console.error(error));
  }

  checkScreenings() {
    this.checkingProcess = true;
    this.employeeScreeningService.checkScreenings(this.commonEmail).subscribe(result => {
      this.checkingProcess = false;
      this.loadGrid();
    }, error => console.error(error));
  }

  passTest(testId: number) {
    this.passingProcess = true;
    this.employeeScreeningService.passTest(testId).subscribe(result => {
      this.passingProcess = false;
      this.loadGrid();
    }, error => console.error(error));
  }
}

