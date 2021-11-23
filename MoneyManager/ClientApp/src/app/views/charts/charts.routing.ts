import { Routes } from '@angular/router';

import { LineChartComponent } from "./basic-line-chart/charts.component";
import { RefillVsExpensesComponent } from "./expenses-vs-refill-chart/expenses-vs-refill-chart.component";


export const ChartsRoutes: Routes = [
  { path: '', component: LineChartComponent },
  { path: 'expenses-vs-refill', component: RefillVsExpensesComponent}
];
