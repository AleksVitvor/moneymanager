import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import { CurrencyModel } from 'app/shared/models/CurrencyModel';
import { CrudService } from 'app/views/cruds/crud.service';
import { ReportService } from '../report.service';

@Component({
  selector: 'app-charts',
  templateUrl: './expences-refill.component.html'
})
export class ExpensesRefillChartComponent implements OnInit, OnDestroy {
  public getCategoriesSub: Subscription;
  public getCategoriesValuesSub: Subscription;
  public getExpensesRefillsSub: Subscription;
  public getExpensesRefillsDatesSub: Subscription;
  public itemForm: FormGroup;
  currencies: CurrencyModel[];
  constructor(
    private reportService: ReportService,
    private crudService: CrudService,
    private fb: FormBuilder,
    private http: HttpClient
  ) { }
  title = 'Expenses vs. Refill (EUR)';

  sharedChartOptions: any = {
    responsive: true,
    legend: {
      display: false,
      position: 'bottom'
    }
  };
  chartColors: any[] = [{
    backgroundColor: '#3f51b5',
    borderColor: '#3f51b5',
    pointBackgroundColor: '#3f51b5',
    pointBorderColor: '#fff',
    pointHoverBackgroundColor: '#fff',
    pointHoverBorderColor: 'rgba(148,159,177,0.8)'
  }, {
    backgroundColor: '#eeeeee',
    borderColor: '#e0e0e0',
    pointBackgroundColor: '#e0e0e0',
    pointBorderColor: '#fff',
    pointHoverBackgroundColor: '#fff',
    pointHoverBorderColor: 'rgba(77,83,96,1)'
  }, {
    backgroundColor: 'rgba(148,159,177,0.2)',
    borderColor: 'rgba(148,159,177,1)',
    pointBackgroundColor: 'rgba(148,159,177,1)',
    pointBorderColor: '#fff',
    pointHoverBackgroundColor: '#fff',
    pointHoverBorderColor: 'rgba(148,159,177,0.8)'
  }];

  lineChartData: any[] = [{
    data: [5, 5, 7, 8, 4, 5, 5],
    label: 'Series A',
    borderWidth: 1
  }, {
    data: [5, 4, 4, 3, 6, 2, 5],
    label: 'Series B',
    borderWidth: 1
  }];
  lineChartLabels: any[] = ['1', '2', '3', '4', '5', '6', '7', '8'];
  lineChartOptions: any = Object.assign({
    animation: false,
    scales: {
      xAxes: [{
        gridLines: {
          color: 'rgba(0,0,0,0.02)',
          zeroLineColor: 'rgba(0,0,0,0.02)'
        }
      }],
      yAxes: [{
        gridLines: {
          color: 'rgba(0,0,0,0.02)',
          zeroLineColor: 'rgba(0,0,0,0.02)'
        },
        ticks: {
          beginAtZero: true,
          suggestedMax: 9,
        }
      }]
    }
  }, this.sharedChartOptions);
  public lineChartLegend = false;
  public lineChartType = 'line';

  public lineChartClicked(e: any): void {
  }
  public lineChartHovered(e: any): void {
  }

  ngOnInit() {
    this.getMonth();
    this.getExpensesRefill();
    this.getCurrencies();
    this.buildItemForm();
  }
  ngOnDestroy() {
    
  }

  submit() {
    if (!this.itemForm.invalid) {
      let currencyCode = this.currencies.filter(x=>x.value === this.itemForm.value.currencyId)[0].viewValue;
      this.title = 'Expenses vs. Refill (' + currencyCode + ')';

      this.reportService.getMonthByStartAndEnd(this.itemForm.value.startDate, this.itemForm.value.endDate)
      .subscribe(data => {
        this.lineChartLabels = data;
      });

      return this.reportService.getExpensesVsRefillReport(this.itemForm.value.currencyId, this.itemForm.value.startDate, this.itemForm.value.endDate)
        .subscribe(data => {
          this.lineChartData = data;
        }, err => {
        })
    };
  }

  buildItemForm() {
    this.itemForm = this.fb.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      currencyId: ['', Validators.required]
    });
  }

  getExpensesRefill() {
    this.reportService.getExpensesVsRefill()
      .subscribe(data => {
        this.lineChartData = data;
      });
  }

  getCurrencies() {
    this.crudService.getCurrencies()
      .subscribe(data => {
        this.currencies = data;
        this.itemForm.controls['currencyId'].setValue('');
      });
  }

  getMonth() {
    this.reportService.getMonth()
      .subscribe(data => {
        this.lineChartLabels = data;
      });
  }
}
