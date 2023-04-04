import { IPatient } from './../../../core/interfaces/patient.interface';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil} from 'rxjs/operators';
import { AccountService } from '../api/account.service.';
import { DiseaseHistoryComponent } from 'src/app/shared/components/disease-history/disease-history.component';

@Component({
  selector: 'app-patient-account',
  templateUrl: './patient-account.component.html',
  styleUrls: ['./patient-account.component.scss']
})
export class PatientAccountComponent implements OnInit, OnDestroy {


  @Input() patientInfo: IPatient;
  @Input() editable = false;
  @Input() usePaneClass = true;



  patientForm: FormGroup;

  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private accountService: AccountService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.createForm();
    this.initForm();
  }

  onSubmit(): void {
    if (this.patientForm.valid) {
      this.accountService.updatePatientInfo({
        id: this.patientInfo.id,
        userId: this.patientInfo.userId,
        diseaseHistoryId: this.patientInfo.diseaseHistoryId,
        ...this.patientForm.value
      }).pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.toastr.success('Patient info updated');
          Object.assign(this.patientInfo, this.patientForm.value)
          this.patientForm.markAsUntouched();
      });
    } else {
      this.patientForm.markAllAsTouched();
    }
  }

  createForm(): void  {
    this.patientForm = this.formBuilder.group({
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      country: [null, Validators.required],
      passport: [null, Validators.required],
      address: [null, Validators.required],
      phone: [null, Validators.required]
    });
  }

  initForm(): void {
    this.patientForm.get('firstName').setValue(this.patientInfo.firstName);
    this.patientForm.get('lastName').setValue(this.patientInfo.lastName);
    this.patientForm.get('country').setValue(this.patientInfo.country);
    this.patientForm.get('address').setValue(this.patientInfo.address);
    this.patientForm.get('passport').setValue(this.patientInfo.passport);
    this.patientForm.get('phone').setValue(this.patientInfo.phone);
    this.patientForm.markAsUntouched();
  }

  openDiseaseHistory(patientId: number) {
    const config = new MatDialogConfig();
    config.panelClass = `modal-setting`;
    config.maxWidth = '100vw';
    config.maxHeight = '100vh';
    config.height = '100%';
    config.width = '100%';
    config.disableClose = true;
    config.data = { patientId, editable: !this.editable};
    const dialogRef = this.dialog.open(DiseaseHistoryComponent, config);
  }

  openPatientOrgan(id: number): void {
    this.router.navigate(['/artificial-organs/patient-organs/', id]);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
