import { PatientListComponent } from './patient-list/patient-list.component';
import { AccountInfoComponent } from './account-info/account-info.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'info' , pathMatch: 'prefix'},
  { path: 'info', component: AccountInfoComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
