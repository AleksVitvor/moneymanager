import {Component, OnInit, Inject, OnDestroy} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import {TransactionTypeModel} from '../../../../shared/models/transactiontype.model';
import {TransactionCategoryModel} from '../../../../shared/models/transactioncategory.model';
import {Subscription} from 'rxjs';
import {CrudService} from '../../crud.service';

@Component({
  selector: 'app-ngx-table-popup',
  templateUrl: './ngx-table-popup.component.html'
})
export class NgxTablePopupComponent implements OnInit, OnDestroy {
  transactionTypes: TransactionTypeModel[] = [
    {value: 1, viewValue: 'Expenses'},
    {value: 2, viewValue: 'Refill'}
  ];
  transactionCategories: TransactionCategoryModel[];
  public getCateSub: Subscription;
  public itemForm: FormGroup;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<NgxTablePopupComponent>,
    private fb: FormBuilder,
    private crudService: CrudService
  ) { }

  ngOnInit() {
    this.getCategories();
    this.buildItemForm(this.data.payload);
  }

  ngOnDestroy() {
    if (this.getCateSub) {
      this.getCateSub.unsubscribe();
    }
  }

  getCategories() {
      this.crudService.getCategories()
        .subscribe(data => {
          this.transactionCategories = data;
        });
  }

  buildItemForm(item) {
    this.itemForm = this.fb.group({
      storedAmount: [item.storedAmount || '', Validators.required],
      transactionTypeId: [item.transactionTypeId || '', Validators.required],
      transactionCategoryId: [item.transactionCategoryId || '', Validators.required],
      transactionDate: [item.transactionDate || '', Validators.required],
      isRepeatable: [item.isRepeatable || false]
    });
  }

  submit() {
    this.dialogRef.close(this.itemForm.value);
  }
}
