import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { GoogleChartsModule } from 'angular-google-charts';
import { SharedModule } from 'app/shared/shared.module';
import { ChartsModule } from 'ng2-charts';
import { ReportsRoutes } from './reports.routing';
import { ReportService } from './report.service';
import { CrudService } from '../cruds/crud.service';
import { ExpensesRefillChartComponent } from './expences-refills-component/expences-refill.component';
import { CategoryChartComponent } from './category-component/category.component';
import { ChartsComponent } from './chart-component/charts.component';
import { MonthGroupComponentComponent } from './month-group-component/month-group-component.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';



@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    NgxDatatableModule,
    MatDatepickerModule,
    MatInputModule,
    MatIconModule,
    MatCardModule,
    MatExpansionModule,
    MatMenuModule,
    MatSidenavModule,
    MatTableModule,
    MatButtonModule,
    MatChipsModule,
    MatListModule,
    MatTooltipModule,
    MatDialogModule,
    GoogleChartsModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    TranslateModule,
    SharedModule,
    RouterModule.forChild(ReportsRoutes),
    MatSelectModule,
    FormsModule,
    ChartsModule
  ],
  declarations: [CategoryChartComponent, ChartsComponent, ExpensesRefillChartComponent, MonthGroupComponentComponent, MonthGroupComponentComponent],
  providers: [ReportService, CrudService],
})
export class ReportsModule { }
