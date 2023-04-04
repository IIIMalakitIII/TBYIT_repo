import { SharedModule } from 'src/app/shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { MedicalInstituationComponent } from './medical-instituation/medical-instituation.component';
import { UserListComponent } from './user-list/user-list.component';


@NgModule({
  declarations: [MedicalInstituationComponent, UserListComponent],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule
  ]
})
export class AdminModule { }
