import { DiagnoisticInfoComponent } from './../diagnoistic-info/diagnoistic-info.component';
import { IArtificialOrgan } from 'src/app/core/interfaces/artificial-organ.interface';
import { takeUntil } from 'rxjs/internal/operators/takeUntil';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { IPatient } from 'src/app/core/interfaces/patient.interface';
import { ArtificialOrganService } from '../api/artificial-organ.service';
import { DateService } from 'src/app/core/services/date-service';
import { AuthenticationService } from 'src/app/core/services/auth.service';
import { Role } from 'src/app/core/extension/role.enum';

@Component({
  selector: 'app-my-organ',
  templateUrl: './my-organ.component.html',
  styleUrls: ['./my-organ.component.scss']
})
export class MyOrganComponent implements OnInit, OnDestroy {

  patient: IPatient;
  organs: IArtificialOrgan[];
  @Input() patientId: number;
  currentUser;
  historyOpen = true;
  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private dateService: DateService,
              private artificialOrganService: ArtificialOrganService,
              private authService: AuthenticationService,
              private toastr: ToastrService,
              public dialog: MatDialog) {
    authService.currentUser.subscribe( x => this.currentUser = x);
    if (this.currentUser.er_role === Role.Doctor) {
      this.historyOpen = false;
    }
  }

  ngOnInit(): void {
    console.log(this.currentUser, this.patient);
    this.getPatientInfo();

  }

  getPatientInfo(): void {
    let patientId = this.patientId;
    if (!this.patientId) {
      patientId = +this.route.snapshot.paramMap.get('id');
    }

    this.artificialOrganService.getPatientArtificialOrgans(patientId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        res.forEach(element => element.organDatas
          .forEach(o => {
            o.createdAt = this.dateService.utcToLocal(o.createdAt?.toString());
            o.extendedData = JSON.parse(o.extendedData);
          }));
        this.organs = res;
      });
  }

  diagnosticOrgan(organId: number): void {
    this.artificialOrganService.diagnosticOrgan(organId, this.dateService.lastDayOfCurrentMonth().toISOString(),
    this.dateService.tomorrow().toISOString())
    .pipe(takeUntil(this.destroy$))
    .subscribe(res => {
      if (res) {
        this.openDialog(res);
      }
      this.toastr.warning('No result');
    });
  }

  openDialog(res): void {
    const config = new MatDialogConfig();
    config.panelClass = `modal-setting`;
    config.maxWidth = '60vw';
    config.maxHeight = '60vh';
    config.height = '100%';
    config.width = '100%';
    config.disableClose = true;
    config.data = { text: res };
    this.dialog.open(DiagnoisticInfoComponent, config);
  }

  createNewOrganKey(organId): void {
    this.artificialOrganService.generateKeys(organId)
    .pipe(takeUntil(this.destroy$))
    .subscribe(() => {
      this.toastr.success('Organ keys generated');
    });
  }

  openDetailedInfo(id): void {
    this.router.navigate(['/artificial-organs/detailed-data/', id]);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
