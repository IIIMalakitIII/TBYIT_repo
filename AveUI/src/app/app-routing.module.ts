import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Role } from './core/extension/role.enum';
import { PatientListComponent } from './features/account/patient-list/patient-list.component';
import { AuthGuard } from './guard/auth.guard';
import { RoleGuard } from './guard/role-guard';
import { ControlsTestComponent } from './shared/components/controls-test/controls-test.component';


const routes: Routes = [
  {
    path: '',
    redirectTo: 'account',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'artificial-organs',
    loadChildren: () =>
      import('./features/artificial-organ/artificial-organ.module').then(m => m.ArtificialOrganModule),
      data: { roles: [Role.Patient, Role.Doctor], tryToRedirect: 'auth' }, canActivate: [AuthGuard, RoleGuard]
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./features/account/account.module').then(m => m.AccountModule),
      canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./features/admin/admin.module').then(m => m.AdminModule),
      data: { roles: [Role.Admin], tryToRedirect: 'auth' }, canActivate: [AuthGuard, RoleGuard]
  },
  {
    path: 'control', component: ControlsTestComponent,
    canActivate: [AuthGuard],
  },
  { path: 'patient-list', component: PatientListComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
