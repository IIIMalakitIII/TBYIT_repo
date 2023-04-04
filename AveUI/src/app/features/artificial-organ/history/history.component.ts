import { Component, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IConnectHistory } from 'src/app/core/interfaces/connect-history';
import { ArtificialOrganService } from '../api/artificial-organ.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit, OnDestroy {

  @Input() organId;

  userdeleted = false;
  displayedColumns: string[] = ['position', 'description', 'createdAt'];
  dataSource: IConnectHistory[] = [];

  private destroy$ = new Subject<void>();
  constructor(public translateService: TranslateService,
              private artificialOrganService: ArtificialOrganService,
              public dialog: MatDialog,
              public toastr: ToastrService) { }

  ngOnInit(): void {
    this.getConnectHistory();
  }

  getConnectHistory(): void {
    this.artificialOrganService.getConnectHistory(this.organId)
    .pipe(takeUntil(this.destroy$))
    .subscribe(
      data => {
        this.dataSource = data;
      },
      () => {
        this.toastr.error(this.translateService.instant('Something-Is-Wrong'), this.translateService.instant('Error'));
      }
    );
  }

  getStringLocaleDate(date) {
    return new Date(date).toLocaleDateString();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
