import { Signup2Component } from './signup2/signup2.component';
import { Routes } from "@angular/router";

import { ForgotPasswordComponent } from "./forgot-password/forgot-password.component";
import { LockscreenComponent } from "./lockscreen/lockscreen.component";
import { NotFoundComponent } from "./not-found/not-found.component";
import { ErrorComponent } from "./error/error.component";
import { Signin2Component } from './signin2/signin2.component';

export const SessionsRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "signup",
        component: Signup2Component,
      },
      {
        path: "signin",
        component: Signin2Component,
      },
      {
        path: "forgot-password",
        component: ForgotPasswordComponent,
      },
      {
        path: "lockscreen",
        component: LockscreenComponent,
      },
      {
        path: "404",
        component: NotFoundComponent,
      },
      {
        path: "error",
        component: ErrorComponent,
      }
    ]
  }
];
