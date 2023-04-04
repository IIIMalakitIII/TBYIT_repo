import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormGroupDirective, FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject } from 'rxjs';
import { takeUntil, startWith, map } from 'rxjs/operators';
import { DomService } from 'src/app/core/extension/domService.service';
import { IMedicalInstitution } from 'src/app/core/interfaces/medical-Institution.interface';
import { AdminService } from '../api/admin.service';

@Component({
  selector: 'app-medical-instituation',
  templateUrl: './medical-instituation.component.html',
  styleUrls: ['./medical-instituation.component.scss']
})
export class MedicalInstituationComponent implements OnInit, OnDestroy {


  @ViewChild(FormGroupDirective) formDirective: FormGroupDirective;

  medicalInstituationForm: FormGroup;
  medicalInstituationControl: FormControl = new FormControl(null);

  updateState = false;
  medicalInstituations: IMedicalInstitution[] = [];
  medicalInstituationOptions: Observable<IMedicalInstitution[]>;

  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private adminService: AdminService,
              private domService: DomService,
              private toastr: ToastrService) {
    }

  ngOnInit(): void {
    this.createForm();
    this.getmedicalInstituations();
  }

  getmedicalInstituations(): void {
    this.adminService.getAllMedicalInstitutions()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.medicalInstituations = res;
        this.initmedicalInstituationFilter();
      });
  }

  createForm(): void {
    this.medicalInstituationForm = this.formBuilder.group({
      id: 0,
      name: [null, Validators.required],
      country: [null, Validators.required],
      city: [null, Validators.required],
      address: [null, Validators.required],
    });
  }

  clearForm(): void {
    this.updateState = false;
    this.formDirective.resetForm();
    this.medicalInstituationControl.reset();
    this.medicalInstituationForm.reset();
    this.medicalInstituationForm.get('id').setValue(0);
  }

  onSubmit(): void {
    if (this.medicalInstituationForm.valid) {
      this.updateState ? this.update() : this.create();
    } else {
      this.medicalInstituationForm.markAllAsTouched();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  create(): void {
    this.adminService.createMedicalInstituation(this.medicalInstituationForm.value)
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.clearForm();
        this.getmedicalInstituations();
      });
  }

  update(): void {
    this.adminService.updateMedicalInstituatio(this.medicalInstituationForm.value)
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.clearForm();
        this.getmedicalInstituations();
      });
  }

  fillFormFoUpdate(category: IMedicalInstitution): void {
    this.clearForm();
    this.updateState = true;
    this.medicalInstituationForm.get('id').setValue(category.id);
    this.medicalInstituationForm.get('name').setValue(category.name);
    this.medicalInstituationForm.get('country').setValue(category.country);
    this.medicalInstituationForm.get('city').setValue(category.city);
    this.medicalInstituationForm.get('address').setValue(category.address);
  }

  private initmedicalInstituationFilter() {
    this.medicalInstituationOptions = this.medicalInstituationControl.valueChanges
    .pipe(
      startWith(null),
      map(state => state ? this._filtermedicalInstituation(state) : this.medicalInstituations.slice())
    );
  }


  private _filtermedicalInstituation(value): any[] {
    const filterValue = value && value.name ? value.name.toLowerCase() : value?.toLowerCase();

    return this.medicalInstituations.filter(element => element.name.toLowerCase().includes(filterValue));
  }


  exportDataAsExcel(): void {
    this.adminService.exportDataAsExcel()
    .subscribe((collis: any) => {
      if (collis) {
        this.domService.downloadFile(`excel data ${new Date().toLocaleDateString()}.xlsx`, collis, 'application/xlsx;base64');
      }
    });
  }
}
