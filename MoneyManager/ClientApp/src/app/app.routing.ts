import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './shared/components/layouts/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from './shared/components/layouts/auth-layout/auth-layout.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { UserRoleGuard } from './shared/guards/user-role.guard';

export const rootRouterConfig: Routes = [
  {
    path: '',
    redirectTo: 'transaction',
    pathMatch: 'full'
  },
  {
    path: '',
    component: AuthLayoutComponent,
    children: [
      {
        path: 'sessions',
        loadChildren: () => import('./views/sessions/sessions.module').then(m => m.SessionsModule),
      }
    ]
  },
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'transaction',
        loadChildren: () => import('app/views/cruds/cruds.module').then(m => m.CrudsModule)
      },
      {
        path: 'report',
        loadChildren: () => import('app/views/reports/reports.module').then(m => m.ReportsModule)
      },
      {
        path: 'users',
        canActivate: [UserRoleGuard],
        data: { roles: 'Admin' },
        loadChildren: () => import('app/views/user-controls/user-management.module').then(m => m.UserManagementModule)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'sessions/404'
  }
];

