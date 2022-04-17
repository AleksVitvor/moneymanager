import { Routes } from "@angular/router";
import { UserRoleGuard } from "../../shared/guards/user-role.guard";
import { UserManagementComponent } from "./user-control-component/user-management.component";

export const UserManagementRoutes: Routes = [
  {
    path: '',
    component: UserManagementComponent,
    canActivate: [UserRoleGuard],
    data: { title: 'User Management', roles: 'Admin' }
  }
];
