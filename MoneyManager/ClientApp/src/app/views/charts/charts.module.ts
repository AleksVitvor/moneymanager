import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ChartsModule } from 'ng2-charts';
import { NgxEchartsModule } from 'ngx-echarts';

import { ChartsRoutes } from "./charts.routing";
import { LineChartComponent } from "./basic-line-chart/charts.component";
import { ChartService } from "./charts.service";
import { RefillVsExpensesComponent } from "./expenses-vs-refill-chart/expenses-vs-refill-chart.component";

@NgModule({
  imports: [
    CommonModule,
    MatListModule,
    MatCardModule,
    FlexLayoutModule,
    ChartsModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
    RouterModule.forChild(ChartsRoutes)
  ],
  declarations: [LineChartComponent, RefillVsExpensesComponent],
  providers: [ChartService]
})
export class AppChartsModule { }
