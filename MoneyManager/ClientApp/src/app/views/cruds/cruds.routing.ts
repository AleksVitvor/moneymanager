import { Routes } from '@angular/router';
import { CrudNgxTableComponent } from './crud-ngx-table/crud-ngx-table.component';
import { ChartsComponent } from './reports/charts.component';
import { CategoryChartComponent } from './reports/category-component/category.component'
import { ExpensesRefillChartComponent } from './reports/expences-refills-component/expences-refill.component';

export const CrudsRoutes: Routes = [
  {
    path: '',
    component: CrudNgxTableComponent
  },
  {
    path: 'report',
    component: ChartsComponent,
    data: { title: 'Report' }
  },
  {
    path: 'report/category',
    component: CategoryChartComponent,
    data: { title: 'Category Report' }
  },
  {
    path: 'report/expenses-vs-refill',
    component: ExpensesRefillChartComponent,
    data: { title: 'Expenses vs. Refill' }
  } 
];
