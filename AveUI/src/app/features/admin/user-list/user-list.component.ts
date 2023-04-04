import { IFullUserInfo } from 'src/app/core/interfaces/user.interface';
import { AccountService } from './../../account/api/account.service.';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit, OnDestroy {
  userdeleted = false;
  displayedColumns: string[] = ['position', 'email', 'userName', 'fullName', 'role', 'action'];
  dataSource: IFullUserInfo[] = [];

  private destroy$ = new Subject<void>();
  constructor(public  translateService: TranslateService,
              private accountService: AccountService,
              public toastr: ToastrService) { }

  ngOnInit(): void {
    this.getUserList();
  }

  deleteUser(userId: string) {
    this.userdeleted = true;
    this.accountService.deleteUser(userId)
      .pipe(
        finalize(() => {
          this.userdeleted = false;
          this.getUserList();
        }),
        takeUntil(this.destroy$))
      .subscribe(
        () => {
          this.toastr.success(this.translateService.instant('User-Deleted'), this.translateService.instant('Success'));
        },
        () => {
          this.toastr.error(this.translateService.instant('Something-Is-Wrong'), this.translateService.instant('Error'));
        }
      );
  }

  getUserList() {
    this.accountService.getAllUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.dataSource = res;
        },
        () => {
          this.toastr.error(this.translateService.instant('Something-Is-Wrong'), this.translateService.instant('Error'));
        }
      );
  }

  openUser(userId: number): void {

  }

  getRoleName(userInfo: IFullUserInfo): string {
    if (!userInfo.patient && !userInfo.doctor) {
      return 'Admin';
    }

    if (userInfo.patient) {
      return 'Patient';
    }

    return 'Doctor';
  }

  getFullName(userInfo: IFullUserInfo): string {
    if (!userInfo.patient && !userInfo.doctor) {
      return '';
    }

    return userInfo.patient
      ? userInfo.patient.firstName + ' ' + userInfo.patient.lastName
      : userInfo.doctor.firstName + ' ' + userInfo.doctor.lastName;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
