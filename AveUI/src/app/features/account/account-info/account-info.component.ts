import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AccountService } from '../api/account.service.';
import { IAccountInfo } from 'src/app/core/interfaces/account-info.interface';

@Component({
  selector: 'app-account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.scss']
})
export class AccountInfoComponent implements OnInit, OnDestroy {


  accountInfo: IAccountInfo;
  accountForm: FormGroup;

  private destroy$ = new Subject<void>();
  constructor(private formBuilder: FormBuilder,
              private accountService: AccountService,
              private toastr: ToastrService,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getAccountInfo();
  }

  getAccountInfo(): void {
    this.accountService.getAccountInfo()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res) {
          this.accountInfo = res;
          this.createForm();
          this.initForm();
        }
      });
  }

  onSubmit(): void {
    if (this.accountForm.valid) {
      this.accountService.updateAccountInfo(this.accountForm.value)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.toastr.success('AccountInfo update succesful!');
          this.accountForm.markAsUntouched();
          Object.assign(this.accountInfo, this.accountForm.value);
        });
    } else {
      this.accountForm.markAllAsTouched();
    }
  }

  createForm(): void  {
    this.accountForm = this.formBuilder.group({
      userName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
    });
  }

  initForm(): void {
    this.accountForm.get('userName').setValue(this.accountInfo.userName);
    this.accountForm.get('email').setValue(this.accountInfo.email);
    this.accountForm.markAsUntouched();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
