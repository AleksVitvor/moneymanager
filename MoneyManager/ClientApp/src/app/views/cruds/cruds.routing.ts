import { Routes } from '@angular/router';
import { CrudNgxTableComponent } from './crud-ngx-table/crud-ngx-table.component';
import { ChartsComponent } from './reports/charts.component';

export const CrudsRoutes: Routes = [
  {
    path: '',
    component: CrudNgxTableComponent
  },
  {
    path: 'report',
    component: ChartsComponent,
    data: { title: 'Report' }
  }
];
