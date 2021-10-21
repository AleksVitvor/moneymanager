import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

@Injectable()
export class CrudService {
  items: any[];
  constructor(
    private http: HttpClient
  ) {
  }

  getItems(): Observable<any> {
    return this.http.get(`/api/transaction`);
  }
  addItem(item): Observable<any> {
    return this.http.post('/api/transaction', item);
  }
  updateItem(id, item): Observable<any> {
    item.id = id;
    return this.http.put('/api/transaction', item);
  }
  removeItem(row): Observable<any> {
    return this.http.delete('/api/transaction?id=' + row.id);
  }

  getCategories() : Observable<any> {
    return this.http.get(`/api/transactioncategories`);
  }
}
