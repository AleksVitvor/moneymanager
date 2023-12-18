import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class UserManagementService {
  items: any[];
  constructor(
    private http: HttpClient
  ) {
  }

  getItems(): Observable<any> {
    return this.http.get(`/api/user`);
  }

  chageUserActive(id): Observable<any> {
    return this.http.put(`/api/user?id=${id}`, null);
  }

  updateUserRole(id): Observable<any> {
    return this.http.put(`/api/role?id=${id}`, null);
  }
}
