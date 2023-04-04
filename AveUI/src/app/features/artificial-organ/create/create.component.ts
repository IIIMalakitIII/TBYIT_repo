import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DiseaseHistoryComponent } from 'src/app/shared/components/disease-history/disease-history.component';
import { ArtificialOrganService } from '../api/artificial-organ.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit, OnDestroy {

  organForm: FormGroup;
  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              public dialogRef: MatDialogRef<CreateComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private artificialOrganService: ArtificialOrganService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.createForm();
  }

  onSubmit(): void {
    if (this.organForm.valid) {
      this.artificialOrganService.addArtificialOrgan({
        id: 0,
        patientId: this.data.patientId,
        ...this.organForm.value
      }).pipe(takeUntil(this.destroy$))
        .subscribe(res => {
          this.toastr.success('Organ succesfully added');
          this.dialogRef.close();
          this.artificialOrganService.generateKeys(res.id)
            .pipe(takeUntil(this.destroy$))
            .subscribe(() => {});
        });
    } else {
      this.organForm.markAllAsTouched();
    }
  }

  createForm(): void  {
    this.organForm = this.formBuilder.group({
      name: [null, Validators.required],
      organType: [null, Validators.required],
      description: [null, Validators.required],
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
