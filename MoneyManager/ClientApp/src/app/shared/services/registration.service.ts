import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegistrationModel } from '../models/registration.model';

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private http: HttpClient) { }

  register(user: RegistrationModel) {
    return this.http.post(`api/register`, user);
  }
}
