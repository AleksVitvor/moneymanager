import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

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

  getSelectedCategories(itemList): Observable<any> {
    return this.http.post('/api/usercategory',
      {
        CategoriesList: itemList
      });
  } 

  addCategory(item): Observable<any> {
    return this.http.post('/api/transactioncategories',
      {
        newCategory: item
      });
  }

  removeCategory(itemId): Observable<any> {
    return this.http.delete('/api/transactioncategories?categoryId=' + itemId);
  }

  getCurrencies(): Observable<any> {
    return this.http.get(`/api/currency`);
  }
}
