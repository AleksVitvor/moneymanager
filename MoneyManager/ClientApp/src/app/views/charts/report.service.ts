import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ChartService {
  items: any[];
  constructor(
    private http: HttpClient
  ) {
  }
}
