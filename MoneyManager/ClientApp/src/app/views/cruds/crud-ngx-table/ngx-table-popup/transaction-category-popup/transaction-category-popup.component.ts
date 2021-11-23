import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TransactionTypeModel } from "../../../../../shared/models/transactiontype.model";
import { TransactionCategoryModel } from "../../../../../shared/models/transactioncategory.model";
import { CrudService } from "../../../crud.service";


@Component({
  selector: "dialog-component",
  templateUrl: './transaction-category-popup.component.html'
})
export class TransactionCategoryComponent {
  public option: string;
  public duplicated: boolean = false;

  constructor(
    private matRef: MatDialogRef<TransactionCategoryComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
  }

  ngOnInit() {}

  cancel() {
    this.matRef.close();
  }

  checkDuplicate() {
    if (this.data.options) {
      setTimeout(() => {
        this.duplicated = this.data.options.some(x => x.viewValue === this.option);
        },
        100);
    } else {
      this.duplicated = false;
    }
  }
}
