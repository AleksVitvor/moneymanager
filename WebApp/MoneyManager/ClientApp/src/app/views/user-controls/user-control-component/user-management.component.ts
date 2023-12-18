import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { egretAnimations } from '../../../shared/animations/egret-animations';
import { UserControlModel } from '../../../shared/models/usercontrol.model';
import { AppConfirmService } from '../../../shared/services/app-confirm/app-confirm.service';
import { AppLoaderService } from '../../../shared/services/app-loader/app-loader.service';
import { UserManagementService } from '../user-management.service';

@Component({
  selector: 'app-user-controls',
  templateUrl: './user-management.component.html',
  animations: egretAnimations
})
export class UserManagementComponent implements OnInit, OnDestroy {
  public items: UserControlModel[];
  public getItemSub: Subscription;
  constructor(
    private snack: MatSnackBar,
    private confirmService: AppConfirmService,
    private userService: UserManagementService,
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
    this.getItemSub = this.userService.getItems()
      .subscribe(
        data => {
          this.items = data;
        },
        error => {
          this.snack.open(error.error, 'OK', { duration: 4000 })
        });
  }

  changeRole(row) {
    this.confirmService.confirm({ message: `Change user role?` })
      .subscribe(res => {
        if (res) {
          this.loader.open();
          this.userService.updateUserRole(row.id)
            .subscribe(
              data => {
                this.items = data;
                this.loader.close();
                this.snack.open('User role changed!', 'OK', { duration: 4000 })
              },
              error => {
                this.loader.close();
                this.snack.open(error.error.message, 'OK', { duration: 4000 })
              }
            );
        }
      })
  }

  changeUserActivation(row) {
    this.confirmService.confirm({ message: `Change user status?` })
      .subscribe(res => {
        if (res) {
          this.loader.open();
          this.userService.chageUserActive(row.id)
            .subscribe(
              data => {
                this.items = data;
                this.loader.close();
                this.snack.open('User status changed!', 'OK', { duration: 4000 })
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
