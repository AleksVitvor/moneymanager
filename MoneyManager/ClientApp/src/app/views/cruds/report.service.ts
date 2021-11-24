import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable()
export class ReportService {
  items: any[];
  constructor(
    private http: HttpClient
  ) {
  }

  getCategoriesValues(): Observable<any> {
    return this.http.get(`/api/categoryreport`);
  }

  getMonth(): Observable<any> {
    return this.http.get(`/api/month`);
  }

  getExpensesVsRefill(): Observable<any> {
    return this.http.get(`/api/expensesrefill`);
  }
}
