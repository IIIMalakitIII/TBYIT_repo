import { HistoryComponent } from './history/history.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArtificialOrganRoutingModule } from './artificial-organ-routing.module';
import { CreateComponent } from './create/create.component';
import { PatientListOrgansComponent } from './patient-list-organs/patient-list-organs.component';
import { MyOrganComponent } from './my-organ/my-organ.component';
import { DiagnoisticInfoComponent } from './diagnoistic-info/diagnoistic-info.component';
import { DetailedDataComponent } from './detailed-data/detailed-data.component';
import { ChartsModule } from 'ng2-charts';


@NgModule({
  declarations: [CreateComponent, PatientListOrgansComponent, MyOrganComponent, DiagnoisticInfoComponent, HistoryComponent, DetailedDataComponent],
  imports: [
    CommonModule,
    ArtificialOrganRoutingModule,
    SharedModule,
    ChartsModule
  ]
})
export class ArtificialOrganModule { }
