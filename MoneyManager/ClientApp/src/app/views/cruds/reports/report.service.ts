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

  getMonthByStartAndEnd(startDate, endDate): Observable<any> {
    return this.http.post('/api/month',
      {
        startDate: startDate,
        endDate: endDate
      });
  }

  getExpensesVsRefillReport(currencyId, startDate, endDate): Observable<any> {
    return this.http.post('/api/expensesrefill',
      {
        currencyId: currencyId,
        startDate: startDate,
        endDate: endDate
      });
  }

  getExpensesVsRefill(): Observable<any> {
    return this.http.get(`/api/expensesrefill`);
  }
}
