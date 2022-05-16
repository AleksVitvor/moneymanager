import { Routes } from "@angular/router";
import { CategoryChartComponent } from "./category-component/category.component";
import { ChartsComponent } from "./chart-component/charts.component";
import { ExpensesRefillChartComponent } from "./expences-refills-component/expences-refill.component";
import { MonthGroupComponentComponent } from "./month-group-component/month-group-component.component";

export const ReportsRoutes: Routes = [
  {
    path: '',
    component: ChartsComponent,
    data: { title: 'Report' }
  },
  {
    path: 'category',
    component: CategoryChartComponent,
    data: { title: 'Category Report' }
  },
  {
    path: 'expenses-vs-refill',
    component: ExpensesRefillChartComponent,
    data: { title: 'Expenses vs. Refill' }
  },
  {
    path: 'total',
    component: MonthGroupComponentComponent,
    data: { title: 'Total by month' }
  }
];
