import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { CurrencyModel } from 'app/shared/models/CurrencyModel';
import { CrudService } from 'app/views/cruds/crud.service';

import * as _moment from 'moment';

import {default as _rollupMoment, Moment} from 'moment';
import { ReportService } from '../report.service';

const moment = _rollupMoment || _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-month-group-component',
  templateUrl: './month-group-component.component.html',
  styleUrls: ['./month-group-component.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class MonthGroupComponentComponent implements OnInit {
  @ViewChild('outerSort', { static: true }) sort: MatSort;
  @ViewChildren('innerSort') innerSort: QueryList<MatSort>;
  @ViewChildren('innerTables') innerTables: QueryList<MatTable<Transaction>>;

  dataSource: MatTableDataSource<TransactionTotalReport>;
  usersData: TransactionTotalReport[] = [];
  
  columnsToDisplay = ['Total', 'Refill', 'Expenses', 'Currency', 'Month'];
  innerDisplayedColumns = ['Amount', 'Category', 'Type', 'Date', 'Currency'];
  expandedElement: TransactionTotalReport | null;
  public itemForm: FormGroup;
  currencies: CurrencyModel[];

  transactions: TransactionTotalReport[];

  constructor(
    private reportService: ReportService,
    private cd: ChangeDetectorRef,
    private fb: FormBuilder,
    private crudService: CrudService
  ) { }

  ngOnInit() {
    this.getCurrencies();
    this.getTotalReport();
    this.buildItemForm();
  }

  getCurrencies() {
    this.crudService.getCurrencies()
      .subscribe(data => {
        this.currencies = data;
        this.itemForm.controls['currencyId'].setValue('');
      });
  }

  populateTable() {
    this.usersData = [];
    this.transactions.forEach(user => {
      if (user.addresses && Array.isArray(user.addresses) && user.addresses.length) {
        this.usersData = [...this.usersData, { ...user, addresses: new MatTableDataSource(user.addresses) }];
      } else {
        this.usersData = [...this.usersData, user];
      }
    });
    this.dataSource = new MatTableDataSource(this.usersData);
    this.dataSource.sort = this.sort;
  }

  getTotalReport() {
    this.reportService.getTotalReport()
      .subscribe(data => {
        let allTransactions: TransactionTotalReport[] = [];
        data.forEach(transactionGroup => {
          let monthTransactions: Transaction[] = [];
          transactionGroup.monthTransactions.forEach(transaction => {
            let monthTransaction: Transaction = {
              Amount: transaction.amount,
              Category: transaction.category,
              Type: transaction.type,
              Date: transaction.date,
              Currency: transaction.currency
            };

            monthTransactions.push(monthTransaction);
          });

          let transaction: TransactionTotalReport = {
            Total: transactionGroup.total,
            Refill: transactionGroup.refill,
            Expenses: transactionGroup.expenses,
            Currency: transactionGroup.currency,
            Month: transactionGroup.month,
            addresses: monthTransactions
          }

          allTransactions.push(transaction);
        });
        this.transactions = allTransactions;
        this.populateTable();
      });
  }

  buildItemForm() {
    this.itemForm = this.fb.group({
      startDate: [moment(), Validators.required],
      endDate: [moment(), Validators.required],
      currencyId: ['', Validators.required]
    });

    const ctrlValue = this.itemForm.controls['startDate'].value;
    ctrlValue._d.setYear(new Date().getFullYear() - 1);
    this.itemForm.controls['startDate'].setValue(ctrlValue);
  }

  submit() {
    if (!this.itemForm.invalid) {
      return this.reportService.getTotalReportParams(this.itemForm.value.currencyId, this.itemForm.value.startDate, this.itemForm.value.endDate)
        .subscribe(data => {
          let allTransactions: TransactionTotalReport[] = [];
          data.forEach(transactionGroup => {
            let monthTransactions: Transaction[] = [];
            transactionGroup.monthTransactions.forEach(transaction => {
              let monthTransaction: Transaction = {
                Amount: transaction.amount,
                Category: transaction.category,
                Type: transaction.type,
                Date: transaction.date,
                Currency: transaction.currency
              };

              monthTransactions.push(monthTransaction);
            });

            let transaction: TransactionTotalReport = {
              Total: transactionGroup.total,
              Refill: transactionGroup.refill,
              Expenses: transactionGroup.expenses,
              Currency: transactionGroup.currency,
              Month: transactionGroup.month,
              addresses: monthTransactions
            }

            allTransactions.push(transaction);
          });
          this.transactions = allTransactions;
          this.populateTable();
        }, err => {
        })
    }
  }

  toggleRow(element: TransactionTotalReport) {
    element.addresses && (element.addresses as MatTableDataSource<Transaction>).data.length ? (this.expandedElement = this.expandedElement === element ? null : element) : null;
    this.cd.detectChanges();
    this.innerTables.forEach((table, index) => (table.dataSource as MatTableDataSource<Transaction>).sort = this.innerSort.toArray()[index]);
  }

  applyFilter(filterValue: string) {
    this.innerTables.forEach((table, index) => (table.dataSource as MatTableDataSource<Transaction>).filter = filterValue.trim().toLowerCase());
  }

  chosenStartYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.itemForm.controls['startDate'].value;
    ctrlValue._d.setYear(normalizedYear.year());
    this.itemForm.controls['startDate'].setValue(ctrlValue);
  }

  chosenStartMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.itemForm.controls['startDate'].value;
    ctrlValue._d.setMonth(normalizedMonth.month());
    this.itemForm.controls['startDate'].setValue(ctrlValue);
    datepicker.close();
  }

  chosenEndYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.itemForm.controls['endDate'].value;
    ctrlValue._d.setYear(normalizedYear.year());
    this.itemForm.controls['endDate'].setValue(ctrlValue);
  }

  chosenEndMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.itemForm.controls['endDate'].value;
    ctrlValue._d.setMonth(normalizedMonth.month());
    this.itemForm.controls['endDate'].setValue(ctrlValue);
    datepicker.close();
  }
}

export interface TransactionTotalReport {
  Total: number;
  Refill: number;
  Expenses: number;
  Currency: string;
  Month: string;
  addresses?: Transaction[] | MatTableDataSource<Transaction>;
}

export interface Transaction {
  Amount: number;
  Category: string;
  Type: string;
  Date: string;
  Currency: string;
}
