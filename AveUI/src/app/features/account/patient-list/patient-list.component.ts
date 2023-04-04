import { IPatient } from './../../../core/interfaces/patient.interface';
import { takeUntil } from 'rxjs/internal/operators/takeUntil';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { AccountService } from '../api/account.service.';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.scss']
})
export class PatientListComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  searchControl = new FormControl(null);
  patientList: IPatient[] = [];
  constructor(private formBuilder: FormBuilder,
              private accountService: AccountService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.searchControl.valueChanges
    .pipe(takeUntil(this.destroy$))
    .subscribe(res => this.getPatientListByFilter(res));
  }

  getPatientListByFilter(filter: string) {
    this.accountService.getPatientByFilter(filter)
    .pipe(takeUntil(this.destroy$))
    .subscribe(res => this.patientList = res);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
