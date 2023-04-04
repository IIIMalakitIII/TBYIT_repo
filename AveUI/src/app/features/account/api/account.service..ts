import { IFullUserInfo } from 'src/app/core/interfaces/user.interface';
import { IConfidant } from './../../../core/interfaces/confidant.interface';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { IAccountInfo } from 'src/app/core/interfaces/account-info.interface';
import { ISelectInfo } from 'src/app/core/interfaces/select-info.interface';
import { IPatient } from 'src/app/core/interfaces/patient.interface';
import { IDoctor } from 'src/app/core/interfaces/doctor.interface';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) {}

  getAccountInfo(): Observable<IAccountInfo> {
    return this.http.get<IAccountInfo>(environment.apiUrl +  'Account/account-info');
  }

  getConfidantsInfo(): Observable<IConfidant[]> {
    return this.http.get<IConfidant[]>(environment.apiUrl +  'ArtificialOrgan/confidant-info');
  }

  deleteConfidants(id: number): Observable<any> {
    return this.http.delete(environment.apiUrl +  'ArtificialOrgan/delete-confidant/' + id);
  }

  addConfidant(id: number): Observable<any> {
    return this.http.post(environment.apiUrl +  'ArtificialOrgan/confidant/' + id, {});
  }

  updateAccountInfo(model): Observable<any> {
    return this.http.put<any>(environment.apiUrl +  'Account/account-info', model);
  }

  updatePatientInfo(model): Observable<any> {
    return this.http.put<any>(environment.apiUrl +  'Patient/patient-info', model);
  }

  updateDoctorInfo(model): Observable<any> {
    return this.http.put<any>(environment.apiUrl +  'Doctor/doctor-info', model);
  }

  getMedicalInstitutions(): Observable<ISelectInfo[]> {
    return this.http.get<ISelectInfo[]>(environment.apiUrl +  'MedicalInstituation/autocomplete');
  }

  getPatientByFilter(filter: string): Observable<IPatient[]> {
    return this.http.get<IPatient[]>(environment.apiUrl +  'Patient/patinet-by-filter/' + filter);
  }

  getDoctorByFilter(filter: string): Observable<IDoctor[]> {
    return this.http.get<IDoctor[]>(environment.apiUrl +  'Doctor/doctor-by-filter/' + filter);
  }

  getAllUsers(): Observable<IFullUserInfo[]> {
    return this.http.get<IFullUserInfo[]>(environment.apiUrl +  'Account/allUsers');
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete(environment.apiUrl +  'Account/deleteUser?userId=' + userId);
  }

}
