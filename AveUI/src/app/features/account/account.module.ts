import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { PatientAccountComponent } from './patient-account/patient-account.component';
import { DoctorAccountComponent } from './doctor-account/doctor-account.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountInfoComponent } from './account-info/account-info.component';
import { PatientListComponent } from './patient-list/patient-list.component';
import { ConfidantsComponent } from './confidants/confidants.component';


@NgModule({
  declarations: [
    AccountInfoComponent,
    PatientAccountComponent,
    DoctorAccountComponent,
    PatientListComponent,
    ConfidantsComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule
  ]
})
export class AccountModule { }
