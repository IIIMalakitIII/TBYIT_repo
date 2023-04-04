import { IDoctor } from './../../../core/interfaces/doctor.interface';
import { IConfidant } from './../../../core/interfaces/confidant.interface';
import { takeUntil } from 'rxjs/internal/operators/takeUntil';
import { Input, OnDestroy } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { AccountService } from '../api/account.service.';
import { MatSelectChange } from '@angular/material/select';
import { DiseaseHistoryComponent } from 'src/app/shared/components/disease-history/disease-history.component';

@Component({
  selector: 'app-confidants',
  templateUrl: './confidants.component.html',
  styleUrls: ['./confidants.component.scss']
})
export class ConfidantsComponent implements OnInit, OnDestroy {

  confidants: IConfidant[] = [];
  searchControl = new FormControl('');
  doctorList: IDoctor[] = [];
  @Input() isPatient = true;
  @Input() searchActive = true;
  @Input() list = true;

  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private accountService: AccountService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getConfidants();
    this.searchControl.valueChanges
    .pipe(takeUntil(this.destroy$))
    .subscribe(res => this.getDoctorListByFilter(res));
  }

  getDoctorListByFilter(filter: string) {
    if (filter && filter !== '') {
      this.accountService.getDoctorByFilter(filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => this.doctorList = res.filter(x => !this.confidants.some(o => o.doctorId === x.id)));
    }
  }

  getConfidants(): void {
    this.accountService.getConfidantsInfo()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.confidants = res;
        this.getDoctorListByFilter(this.searchControl.value);
      });
  }

  deleteConfidant(id: number): void {
    this.accountService.deleteConfidants(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.toastr.success('Confidant deleted');
        this.getConfidants();
      }, err => {
        this.toastr.error(err.error.Text);
      } );
  }

  addConfidant(id: number): void {
      this.accountService.addConfidant(id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.toastr.success('Confidant added');
          this.getConfidants();
        }, err => {
          this.toastr.error(err.error.Message);
        });
  }

  openPatientOrgan(id: number): void {
    this.router.navigate(['/artificial-organs/patient-organs/', id]);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
