import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ChartsModule } from 'ng2-charts';

import { ChartsComponent } from './base-chart-component/charts.component';
import { ChartsRoutes } from './report.routing';
import { ChartService } from './report.service';


@NgModule({
  imports: [
    CommonModule,
    MatListModule,
    MatCardModule,
    FlexLayoutModule,
    ChartsModule,
    RouterModule.forChild(ChartsRoutes)
  ],
  declarations: [ChartsComponent],
  providers: [ChartService]
})
export class ReportModule { }
