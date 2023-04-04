import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-diagnoistic-info',
  templateUrl: './diagnoistic-info.component.html',
  styleUrls: ['./diagnoistic-info.component.scss']
})
export class DiagnoisticInfoComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
