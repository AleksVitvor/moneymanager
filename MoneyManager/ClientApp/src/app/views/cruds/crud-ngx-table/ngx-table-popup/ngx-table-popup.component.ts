import {Component, OnInit, Inject, OnDestroy} from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TransactionTypeModel } from '../../../../shared/models/transactiontype.model';
import { TransactionCategoryModel } from '../../../../shared/models/transactioncategory.model';
import { Subscription } from 'rxjs';
import { MatIconRegistry } from "@angular/material/icon";
import { DomSanitizer } from "@angular/platform-browser";
import { CrudService } from '../../crud.service';
import { TransactionCategoryComponent } from "./transaction-category-popup/transaction-category-popup.component";
import { CurrencyModel } from "../../../../shared/models/CurrencyModel";
import { repeatableRequired } from 'app/validators/requiredValidator.validator';

@Component({
  selector: 'app-ngx-table-popup',
  templateUrl: './ngx-table-popup.component.html'
})
export class NgxTablePopupComponent implements OnInit, OnDestroy {

  transactionTypes: TransactionTypeModel[] = [
    {value: 1, viewValue: 'Expenses'},
    {value: 2, viewValue: 'Refill'}
  ];

  transactionPeriods: TransactionTypeModel[] = [
    { value: 1, viewValue: 'Once a week' },
    { value: 2, viewValue: 'Once a month' },
    { value: 3, viewValue: 'Once every three month' },
    { value: 4, viewValue: 'Once a year' }
  ];

  transactionCategories: TransactionCategoryModel[];

  currencies: CurrencyModel[];
  public getCategoriesSub: Subscription;
  public updateCategoriesSub: Subscription;
  public removeCategoriesSub: Subscription;
  public getCurrenciesSub: Subscription;
  public itemForm: FormGroup;
  public selectedValue: string;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<NgxTablePopupComponent>,
    private fb: FormBuilder,
    private crudService: CrudService,
    public dialog: MatDialog,
    private matIconRegistry: MatIconRegistry,
    private domSanitizer: DomSanitizer
  ) {
    this.matIconRegistry.addSvgIcon(
      "delete",
      this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/images/svg-icons/close.svg")
    );
  }

  ngOnInit() {
    this.getCategories();
    this.getCurrencies();
    this.buildItemForm(this.data.payload);
  }

  ngOnDestroy() {
    if (this.getCategoriesSub) {
      this.getCategoriesSub.unsubscribe();
    }

    if (this.updateCategoriesSub) {
      this.updateCategoriesSub.unsubscribe();
    }

    if (this.removeCategoriesSub) {
      this.removeCategoriesSub.unsubscribe();
    }

    if (this.getCurrenciesSub) {
      this.getCurrenciesSub.unsubscribe();
    }
  }

  getCategories() {
      this.crudService.getCategories()
        .subscribe(data => {
          this.transactionCategories = data;
          this.itemForm.controls['transactionCategoryId']
            .setValue(this.data.payload.transactionCategory !== undefined ? this.transactionCategories
              .find(x => x.viewValue === this.data.payload.transactionCategory).value : '');
        });
  }

  getCurrencies() {
    this.crudService.getCurrencies()
      .subscribe(data => {
        this.currencies = data;
        this.itemForm.controls['currencyId']
          .setValue(this.data.payload.currencyId !== undefined ? this.currencies
            .find(x => x.value === this.data.payload.currencyId).value : '');
      });
  }

  buildItemForm(item) {
    this.itemForm = this.fb.group({
      storedAmount: [item.storedAmount || '', Validators.required],
      transactionTypeId: [
        item.transactionType !== undefined ? this.transactionTypes.find(x => x.viewValue === item.transactionType).value : '', Validators.required
      ],
      transactionCategoryId: [ '', Validators.required ],
      transactionDate: [item.transactionDate || '', Validators.required],
      isRepeatable: [item.isRepeatable || false],
      currencyId: ['', Validators.required],
      transactionPeriodId: [item.transactionPeriod !== undefined ? this.transactionPeriods.find(x => x.viewValue === item.transactionPeriod).value : '']
    }, { validator: repeatableRequired });
  }

  submit() {
    this.dialogRef.close(this.itemForm.value);
  }

  change() {
    if (this.itemForm.controls['transactionCategoryId'].value === '-1') {
      const dialogRef = this.dialog.open(TransactionCategoryComponent, {
        width: '300px',
        data: { options: this.transactionCategories },
      });
      dialogRef.afterClosed()
        .subscribe(newOption => {
          if (newOption) {
            this.crudService.addCategory(newOption)
              .subscribe(data => {
                this.transactionCategories = data;
              });
            this.itemForm.controls['transactionCategoryId'].setValue('');
          } else {
            this.selectedValue = null;
          }
      })
    }
  }

  deleteOption(ev: Event, option) {
    ev.stopPropagation();
    ev.preventDefault();
    this.crudService.removeCategory(option.value)
      .subscribe(data => {
        this.transactionCategories = data;
      });
    if (option.value === this.itemForm.controls['transactionCategoryId'].value) {
      this.itemForm.controls['transactionCategoryId'].setValue('');
    }
  }
}
