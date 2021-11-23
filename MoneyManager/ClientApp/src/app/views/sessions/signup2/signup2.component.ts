import { CustomValidators } from 'ngx-custom-validators';
import { HttpClient } from "@angular/common/http";
import { Validators, FormGroup, NgForm, FormGroupDirective, FormControl, AbstractControl, ValidationErrors } from "@angular/forms";
import { FormBuilder } from "@angular/forms";
import { Component, OnInit } from "@angular/core";
import { catchError, first, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { JwtAuthService } from '../../../shared/services/auth/jwt-auth.service';
import { throwError } from 'rxjs';

@Component({
  selector: "app-signup2",
  templateUrl: "./signup2.component.html",
  styleUrls: ["./signup2.component.scss"]
})
export class Signup2Component implements OnInit {
  signupForm: FormGroup;
  loading: boolean;

  constructor(
    private fb: FormBuilder,
    jwtAuth: JwtAuthService,
    private http: HttpClient,
    private router: Router) {
    if (jwtAuth.isLoggedIn()) {
      this.router.navigate(['/']);
    }}

  ngOnInit() {

    const password = new FormControl('', [Validators.required, Validators.minLength(6)]);
    const confirmPassword = new FormControl('', CustomValidators.equalTo(password));

    this.signupForm = this.fb.group(
      {
        firstName: ["",Validators.required],
        lastName: ["",Validators.required],
        username: ["",Validators.required],
        email: ["",[Validators.required,Validators.email]],
        password: password,
        confirmPassword: confirmPassword,
        agreed: [false,Validators.required]
      }
    );
  }

  onSubmit() {
    if (!this.signupForm.invalid) {
      return this.http.post('/api/register',
        {
          firstName: this.signupForm.value.firstName,
          lastName: this.signupForm.value.lastName,
          username: this.signupForm.value.username,
          email: this.signupForm.value.email,
          password: this.signupForm.value.password
        })
        .subscribe(response => {
          this.router.navigateByUrl('/sessions/signin');
        }, err => {
          this.loading = false;
        })
    };
  }
}
