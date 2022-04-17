import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { AppLoaderService } from '../../../shared/services/app-loader/app-loader.service';
import { JwtAuthService } from '../../../shared/services/auth/jwt-auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-signin2',
  templateUrl: './signin2.component.html',
  styleUrls: ['./signin2.component.scss']
})
export class Signin2Component implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(MatButton) submitButton: MatButton;

  signinForm: FormGroup;
  errorMsg = '';
  // return: string;

  private _unsubscribeAll: Subject<any>;

  constructor(
    private jwtAuth: JwtAuthService,
    private egretLoader: AppLoaderService,
    private router: Router,
    private snack: MatSnackBar,
    private route: ActivatedRoute
  ) {
    this._unsubscribeAll = new Subject();
  }

  ngOnInit() {
    this.signinForm = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      rememberMe: new FormControl(true)
    });
  }

  ngAfterViewInit() {
    this.autoSignIn();
  }

  ngOnDestroy() {
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }

  signin() {
    const signinData = this.signinForm.value

    this.submitButton.disabled = true;

    if (signinData.email === '' || signinData.password === '') {
      return;
    }

    this.jwtAuth.signin(signinData.email, signinData.password)
      .subscribe(response => {
        this.router.navigateByUrl(this.jwtAuth.return);
      }, err => {
        this.submitButton.disabled = false;
        this.errorMsg = err.error;
        this.snack.open(this.errorMsg, 'Error', { duration: 4000 })
      })
  }

  autoSignIn() {
    if (this.jwtAuth.return === '/') {
      return
    }
    this.egretLoader.open(`Automatically Signing you in! \n Return url: ${this.jwtAuth.return.substring(0, 20)}...`, { width: '320px' });
    setTimeout(() => {
      this.signin();
      console.log('autoSignIn');
      this.egretLoader.close()
    }, 2000);
  }
}
