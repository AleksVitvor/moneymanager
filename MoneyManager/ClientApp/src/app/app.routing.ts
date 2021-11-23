import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './shared/components/layouts/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from './shared/components/layouts/auth-layout/auth-layout.component';
import { AuthGuard } from './shared/guards/auth.guard';

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
        loadChildren: () => import('./views/cruds/cruds.module').then(m => m.CrudsModule)
      }
      //,
      //{
      //  path: 'chart',
      //  loadChildren: () => import('./views/charts/charts.module').then(m => m.AppChartsModule)
      //}
    ]
  },
  {
    path: '**',
    redirectTo: 'sessions/404'
  }
];

