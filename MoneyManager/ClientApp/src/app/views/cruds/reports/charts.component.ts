import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { CrudService } from '../crud.service';
import { ReportService } from '../report.service';

@Component({
  selector: 'app-charts',
  templateUrl: './charts.component.html'
})
export class ChartsComponent implements OnInit, OnDestroy{
  public getCategoriesSub: Subscription;
  public getCategoriesValuesSub: Subscription;
  public getExpensesRefillsSub: Subscription;
  public getExpensesRefillsDatesSub: Subscription;
  constructor(
    private reportService: ReportService,
    private crudService: CrudService
  ) { }
  sharedChartOptions: any = {
    responsive: true,
    // maintainAspectRatio: false,
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

  /*
  * Line Chart Options
  */
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

  /*
  * Radar Chart Options
  */
  public radarChartLabels: string[] = ['Eating', 'Drinking', 'Sleeping', 'Designing', 'Coding', 'Cycling', 'Running'];

  public radarChartData: any = [
    { data: [65, 59, 90, 81, 56, 55, 40], label: 'Series A', borderWidth: 1 },
    { data: [28, 48, 40, 19, 96, 27, 100], label: 'Series B', borderWidth: 1 }
  ];
  public radarChartType = 'radar';
  public radarChartColors: any[] = [
    {
      backgroundColor: 'rgba(36, 123, 160, 0.2)',
      borderColor: 'rgba(36, 123, 160, 0.6)',
      pointBackgroundColor: 'rgba(36, 123, 160, 0.8)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(36, 123, 160, 0.8)'
    },
    {
      backgroundColor: 'rgba(244, 67, 54, 0.2)',
      borderColor: 'rgba(244, 67, 54, .8)',
      pointBackgroundColor: 'rgba(244, 67, 54, .8)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(244, 67, 54, 1)'
    }
  ];

  ngOnInit() {
    this.getCategories();
    this.getMonth();
    this.getValuesByCategories();
    this.getExpensesRefill();
  }
  ngOnDestroy() {
    if (this.getCategoriesSub) {
      this.getCategoriesSub.unsubscribe();
    }
    if (this.getCategoriesValuesSub) {
      this.getCategoriesValuesSub.unsubscribe();
    }
    if (this.getExpensesRefillsDatesSub) {
      this.getExpensesRefillsDatesSub.unsubscribe();
    }
    if (this.getExpensesRefillsSub) {
      this.getExpensesRefillsSub.unsubscribe();
    }
  }

  getCategories() {
    this.crudService.getCategories()
      .subscribe(data => {
        this.radarChartLabels = data.map(function (item) { return item.viewValue });
      });
  }

  getMonth() {
    this.reportService.getMonth()
      .subscribe(data => {
        this.lineChartLabels = data;
      });
  }

  getExpensesRefill() {
    this.reportService.getExpensesVsRefill()
      .subscribe(data => {
        this.lineChartData = data;
      });
  }

  getValuesByCategories() {
    this.reportService.getCategoriesValues()
      .subscribe(data => {
        this.radarChartData = data;
      });
  }

  /*
  * Line Chart Event Handler
  */
  public lineChartClicked(e: any): void {
  }
  public lineChartHovered(e: any): void {
  }

  /*
  * Rader Chart Event Handler
  */
  public radarChartClicked(e: any): void {
  }
  public radarChartHovered(e: any): void {
  }

}
