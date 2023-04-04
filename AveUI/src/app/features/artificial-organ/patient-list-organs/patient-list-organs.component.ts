import { CreateComponent } from './../create/create.component';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { DiseaseHistoryComponent } from 'src/app/shared/components/disease-history/disease-history.component';
import { ArtificialOrganService } from '../api/artificial-organ.service';

@Component({
  selector: 'app-patient-list-organs',
  templateUrl: './patient-list-organs.component.html',
  styleUrls: ['./patient-list-organs.component.scss']
})
export class PatientListOrgansComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  patientId: number;
  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private artificialOrganService: ArtificialOrganService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.patientId = +this.route.snapshot.paramMap.get('id');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  createNewOrgan() {
    const config = new MatDialogConfig();
    config.panelClass = `modal-setting`;
    config.maxWidth = '500px';
    config.maxHeight = '500px';
    config.height = '100%';
    config.width = '100%';
    config.disableClose = true;
    config.data = { patientId: this.patientId };
    this.dialog.open(CreateComponent, config).afterClosed().subscribe(() => {
      window.location.reload();
    });
  }


}
