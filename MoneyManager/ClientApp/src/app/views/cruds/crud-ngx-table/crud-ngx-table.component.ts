import { Component, OnInit, OnDestroy } from '@angular/core';
import { CrudService } from '../crud.service';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AppConfirmService } from '../../../shared/services/app-confirm/app-confirm.service';
import { AppLoaderService } from '../../../shared/services/app-loader/app-loader.service';
import { NgxTablePopupComponent } from './ngx-table-popup/ngx-table-popup.component';
import { Subscription } from 'rxjs';
import { egretAnimations } from "../../../shared/animations/egret-animations";
import { DisplayTransactionModel } from '../../../shared/models/display.transaction.model';

@Component({
  selector: 'app-crud-ngx-table',
  templateUrl: './crud-ngx-table.component.html',
  animations: egretAnimations
})
export class CrudNgxTableComponent implements OnInit, OnDestroy {
  public items: DisplayTransactionModel[];
  public getItemSub: Subscription;
  constructor(
    private dialog: MatDialog,
    private snack: MatSnackBar,
    private crudService: CrudService,
    private confirmService: AppConfirmService,
    private loader: AppLoaderService
  ) { }

  ngOnInit() {
    this.getItems();
  }
  ngOnDestroy() {
    if (this.getItemSub) {
      this.getItemSub.unsubscribe();
    }
  }
  getItems() {
    this.getItemSub = this.crudService.getItems()
      .subscribe(
        data => {
          this.items = data;
        },
        error => {
          this.snack.open(error.error.message, 'OK', { duration: 4000 })
        });
  }

  openPopUp(data: any = {}, isNew?) {
    const title = isNew ? 'Add new transaction' : 'Update transaction';
    const dialogRef: MatDialogRef<any> = this.dialog.open(NgxTablePopupComponent, {
      width: '720px',
      disableClose: true,
      data: { title, payload: data }
    });
    dialogRef.afterClosed()
      .subscribe(res => {
        if (!res) {
          this.getItems();
          return;
        }
        this.loader.open();
        if (isNew) {
          this.crudService.addItem(res)
            .subscribe(
              data => {
                this.items = data;
                this.loader.close();
                this.snack.open('Transaction Added!', 'OK', { duration: 4000 })
              },
              error => {
                this.loader.close();
                this.snack.open(error.error.message, 'OK', { duration: 4000 })
              });
        } else {
          this.crudService.updateItem(data.id, res)
            .subscribe(
              data => {
                this.items = data;
                this.loader.close();
                this.snack.open('Transaction Updated!', 'OK', { duration: 4000 });
              },
              error => {
                this.loader.close();
                this.snack.open(error.error.message, 'OK', { duration: 4000 })
              });
        }
      });
  }
  deleteItem(row) {
    this.confirmService.confirm({message: `Delete transaction?`})
      .subscribe(res => {
        if (res) {
          this.loader.open();
          this.crudService.removeItem(row)
            .subscribe(
              data => {
                this.items = data;
                this.loader.close();
                this.snack.open('Transaction deleted!', 'OK', { duration: 4000 })
              },
              error => {
                this.loader.close();
                this.snack.open(error.error.message, 'OK', { duration: 4000 })
              }
            );
        }
      })
  }
}
