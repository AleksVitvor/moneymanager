import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import { CurrencyModel } from 'app/shared/models/CurrencyModel';
import { TransactionCategoryModel } from 'app/shared/models/transactioncategory.model';
import { CrudService } from 'app/views/cruds/crud.service';
import { ReportService } from '../report.service';
import { TransactionTypeModel } from '../../../shared/models/transactiontype.model';

@Component({
  selector: 'app-charts',
  templateUrl: './category.component.html'
})
export class CategoryChartComponent implements OnInit, OnDestroy {
  transactionTypes: TransactionTypeModel[] = [
    { value: 1, viewValue: 'Expenses' },
    { value: 2, viewValue: 'Refill' }
  ];
  categories = new FormControl();
  public getCategoriesSub: Subscription;
  public getCategoriesValuesSub: Subscription;
  public getExpensesRefillsSub: Subscription;
  public getExpensesRefillsDatesSub: Subscription;
  public itemForm: FormGroup;
  transactionCategories: TransactionCategoryModel[];
  currencies: CurrencyModel[];
  selectedCategories = [];
  constructor(
    private reportService: ReportService,
    private crudService: CrudService,
    private fb: FormBuilder,
    private http: HttpClient
  ) { }
  title = 'Category report (EUR) by Expenses';

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

  public radarChartClicked(e: any): void {
  }
  public radarChartHovered(e: any): void {
  }

  ngOnInit() {
    this.getCategories();
    this.getValuesByCategories();
    this.getCurrencies();
    this.buildItemForm();
  }
  ngOnDestroy() {
    
  }

  submit() {
    if (!this.itemForm.invalid) {
      let currencyCode = this.currencies.filter(x => x.value === this.itemForm.value.currencyId)[0].viewValue;
      let type = this.transactionTypes.filter(x => x.value === this.itemForm.value.transactionTypeId)[0].viewValue;
      this.title = 'Category report (' + currencyCode + ') by ' + type;

      this.crudService.getSelectedCategories(this.selectedCategories)
      .subscribe(data => {
        this.radarChartLabels = data.map(function (item) { return item.viewValue });
      });

      return this.http.post('/api/categoryreport',
        {
          CurrencyId: this.itemForm.value.currencyId,
          StartDate: this.itemForm.value.startDate,
          EndDate: this.itemForm.value.endDate,
          CategoriesList: this.selectedCategories,
          TransactionTypeId: this.itemForm.value.transactionTypeId
        })
        .subscribe(data => {
          this.radarChartData = data;
        }, err => {
        })
    };
  }

  buildItemForm() {
    this.itemForm = this.fb.group({
      startDate: ['', Validators.required],
      transactionCategoryId: [''],
      endDate: ['', Validators.required],
      currencyId: ['', Validators.required],
      transactionTypeId: ['', Validators.required]
    });
  }

  getCategories() {
    this.crudService.getCategories()
      .subscribe(data => {
        this.transactionCategories = data;
        this.itemForm.controls['transactionCategoryId'].setValue('');
        this.radarChartLabels = data.map(function (item) { return item.viewValue });
      });
  }

  getValuesByCategories() {
    this.reportService.getCategoriesValues()
      .subscribe(data => {
        this.radarChartData = data;
      });
  }
  
  getCurrencies() {
    this.crudService.getCurrencies()
      .subscribe(data => {
        this.currencies = data;
        this.itemForm.controls['currencyId'].setValue('');
      });
  }
}
