import { Routes } from '@angular/router';

import { ChartsComponent } from './base-chart-component/charts.component';


export const ChartsRoutes: Routes = [
  { path: '', component: ChartsComponent, data: { title: 'Charts' } }
];
