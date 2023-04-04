import { IDoctor } from './../../../core/interfaces/doctor.interface';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { SystemRecordStatus } from 'src/app/core/extension/record.enum';
import { configureToastr, toastrTitle } from 'src/app/core/helper';
import { IRecord } from 'src/app/core/interfaces/record.interface';
import { ISelectInfo } from 'src/app/core/interfaces/select-info.interface';
import { AccountService } from '../api/account.service.';

@Component({
  selector: 'app-doctor-account',
  templateUrl: './doctor-account.component.html',
  styleUrls: ['./doctor-account.component.scss']
})
export class DoctorAccountComponent implements OnInit, OnDestroy {

  @Input() doctorInfo: IDoctor;
  doctorForm: FormGroup;
  medicalInstitutions: ISelectInfo[] = [];
  medicalInstitutionOptions: Observable<ISelectInfo[]>;
  @Input() editable = false;

  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private accountService: AccountService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.createForm();
    this.initForm();
    this.getMedicalInstitutions();
    configureToastr(this.toastr);
  }

  getMedicalInstitutions(): void {
    this.accountService.getMedicalInstitutions()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.medicalInstitutions = res;
        this.initFilter();
      });
  }

  onSubmit(): void {
    if (this.doctorForm.valid) {
      const medicalInstitutionId = this.doctorForm.get('medicalInstitutionId').value;

      if (!(medicalInstitutionId && medicalInstitutionId?.name)) {
        this.toastr.warning('Please chose medical institution!');
        return;
      }
      this.accountService.updateDoctorInfo({
        id: this.doctorInfo.id,
        userId: this.doctorInfo.userId,
        ...this.doctorForm.value,
        medicalInstitutionId: medicalInstitutionId.id
      })
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.toastr.success('Doctor info updated');
          Object.assign(this.doctorInfo, this.doctorForm.value);
          this.doctorInfo.medicalInstitutionId = medicalInstitutionId.id;
          this.doctorForm.markAsUntouched();
        });
    } else {
      this.doctorForm.markAllAsTouched();
    }
  }

  createForm(): void  {
    this.doctorForm = this.formBuilder.group({
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      license: [null, Validators.required],
      phone: [null, Validators.required],
      address: [null, Validators.required],
      medicalInstitutionId: [null, Validators.required],
    });
  }

  initForm(): void {
    this.doctorForm.get('firstName').setValue(this.doctorInfo.firstName);
    this.doctorForm.get('lastName').setValue(this.doctorInfo.lastName);
    this.doctorForm.get('license').setValue(this.doctorInfo.license);
    this.doctorForm.get('phone').setValue(this.doctorInfo.phone);
    this.doctorForm.get('medicalInstitutionId').setValue(this.doctorInfo.medicalInstitutionId);
    this.doctorForm.markAsUntouched();
  }

  displayFn(element: ISelectInfo): string {
    return element?.name;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initFilter() {
    this.medicalInstitutionOptions = this.doctorForm.get('medicalInstitutionId').valueChanges
    .pipe(
      startWith(null),
      map(state => state ? this._filterStates(state) : this.medicalInstitutions.slice())
    );
  }

  private _filterStates(value): any[] {
    const filterValue = value && value.name ? value.name.toLowerCase() : value?.toLowerCase();

    return this.medicalInstitutions.filter(element => element.name.toLowerCase().indexOf(filterValue) === 0);
  }
}
