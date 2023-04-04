import { IOrganData } from './../../../core/interfaces/organ-data.interface';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import * as pluginAnnotations from 'chartjs-plugin-annotation';
import { takeUntil } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/core/services/auth.service';
import { IFullUserInfo, IUser } from 'src/app/core/interfaces/user.interface';
import { Subject } from 'rxjs';
import { ArtificialOrganService } from '../api/artificial-organ.service';
import { ActivatedRoute } from '@angular/router';
import { DateService } from 'src/app/core/services/date-service';
import { IArtificialOrgan } from 'src/app/core/interfaces/artificial-organ.interface';

@Component({
  selector: 'app-detailed-data',
  templateUrl: './detailed-data.component.html',
  styleUrls: ['./detailed-data.component.scss']
})
export class DetailedDataComponent implements OnInit, OnDestroy {

  generateData: IIndicatorList[] = [];
  indificators: IOrganData[];
  firstCall = false;
  public lineChartData: ChartDataSets[] = [
    { data: [0], label: 'Noun' },
  ];
  public lineChartLabels: Label[] = [];
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    scales: {
      xAxes: [{}],
      yAxes: [
        {
          id: 'y-axis-0',
          position: 'left',
        }
      ]
    },
    annotation: null
  };
  public lineChartColors: Color[] = [
    {
      backgroundColor: 'rgba(255,255,255)',
      borderColor: 'rgba(0,255,169)',
      pointBackgroundColor: 'rgba(0,255,169)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend = true;
  public lineChartType = 'line';
  public lineChartPlugins = [pluginAnnotations];
  currentUser: IUser;
  fullUserInfo: IFullUserInfo;
  organId: number;
  organ: IArtificialOrgan;
  organParams: string[];

  @ViewChild(BaseChartDirective, { static: true }) chart: BaseChartDirective;
  private destroy$ = new Subject<void>();
  constructor(private artificialOrganService: ArtificialOrganService,
              private route: ActivatedRoute,
              private dateService: DateService,
              private authenticationService: AuthenticationService) {
      this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit(): void {
    this.organId = +this.route.snapshot.paramMap.get('id');
    this.getOrganInfo();
  }

  getOrganInfo(): void {
    this.artificialOrganService.getPatientOrgan(this.organId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        res.organDatas
          .forEach(o => {
            o.createdAt = this.dateService.utcToLocal(o.createdAt?.toString());
            o.extendedData = JSON.parse(o.extendedData);
          });
        this.organ = res;
        this.setData();
      });
  }

  setData(): void {
    this.organParams = Object.keys(this.organ.organDatas[0].extendedData);
    this.organParams.forEach(lab => {
      const dataVal = [];
      this.organ.organDatas.forEach((res: IOrganData) => {
        if (res.extendedData[lab]) {
          dataVal.push({
            dataNumber: res.extendedData[lab],
            labelName: res.createdAt
          });
        }
      });

      this.generateData.push(
        {
          display: false,
          paramName: lab,
          indicatorData: dataVal
        }
      );
    });
  }

  displayGrap(paramName: string): void {
    this.firstCall = true;
    this.lineChartColors = [
      {
        backgroundColor: 'rgba(255,255,255)',
        borderColor: 'rgba(0,255,169)',
        pointBackgroundColor: 'rgba(0,255,169)' ,
        pointBorderColor: '#fff',
        pointHoverBackgroundColor: '#fff',
        pointHoverBorderColor: 'rgba(148,159,177,0.8)'
      }
    ];
    this.displayOn(paramName);
    this.getLineChartData(paramName);
    this.getlineChartLabels(paramName);
  }

  getLineChartData(paramName: string): void {
    const dataX = [];
    this.generateData.find(x => x.paramName === paramName)
      .indicatorData.forEach((data: IDotValue) => dataX.push(data.dataNumber));
    this.lineChartData = [{ data: dataX, label: 'Graphs'}];
  }

  getlineChartLabels(paramName: string): void {
    const dataY: Label[] = [];
    this.generateData.find(x => x.paramName === paramName)
      .indicatorData.forEach((data: IDotValue) => dataY.push(data.labelName));
    this.lineChartLabels = dataY;
  }

  displayOn(paramName: string) {
    this.generateData.forEach(x => x.display = false);
    this.generateData.find(x => x.paramName === paramName).display = true;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}

export interface IDotValue {
  dataNumber: number;
  labelName: string;
}

export interface IIndicatorList {
  display: boolean;
  paramName: string;
  indicatorData: IDotValue[];
}
