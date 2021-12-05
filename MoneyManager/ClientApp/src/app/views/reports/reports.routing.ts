import { Routes } from "@angular/router";
import { CategoryChartComponent } from "./category-component/category.component";
import { ChartsComponent } from "./chart-component/charts.component";
import { ExpensesRefillChartComponent } from "./expences-refills-component/expences-refill.component";

export const ReportsRoutes: Routes = [
    {
        path: 'report',
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
    } 
  ];