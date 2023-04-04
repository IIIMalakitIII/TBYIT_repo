import { DetailedDataComponent } from './detailed-data/detailed-data.component';
import { MyOrganComponent } from './my-organ/my-organ.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Role } from 'src/app/core/extension/role.enum';
import { AuthGuard } from 'src/app/guard/auth.guard';
import { RoleGuard } from 'src/app/guard/role-guard';
import { PatientListOrgansComponent } from './patient-list-organs/patient-list-organs.component';


const routes: Routes = [
  { path: '', redirectTo: 'my-organ' , pathMatch: 'prefix'},
  { path: 'my-organs/:id', component: MyOrganComponent, data: {roles: [Role.Patient] }, canActivate: [AuthGuard, RoleGuard]},
  { path: 'detailed-data/:id', component: DetailedDataComponent},
  { path: 'patient-organs/:id', component: PatientListOrgansComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ArtificialOrganRoutingModule { }
