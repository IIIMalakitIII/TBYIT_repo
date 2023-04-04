import { UserListComponent } from './user-list/user-list.component';
import { MedicalInstituationComponent } from './medical-instituation/medical-instituation.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  { path: '', redirectTo: 'med-inst' , pathMatch: 'prefix'},
  { path: 'med-inst', component: MedicalInstituationComponent},
  { path: 'users', component: UserListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
