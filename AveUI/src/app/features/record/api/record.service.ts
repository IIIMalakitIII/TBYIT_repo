import { IPatient } from './../../../core/interfaces/patient.interface';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IRecord } from 'src/app/core/interfaces/record.interface';
import { ISelectInfo } from 'src/app/core/interfaces/select-info.interface';
import { environment } from 'src/environments/environment';
import { IDiseaseHistory } from 'src/app/core/interfaces/disease-history.interface';

@Injectable({
  providedIn: 'root'
})
export class RecordService {

  constructor(private http: HttpClient) {}

  getMedicalInstitutions(): Observable<ISelectInfo[]> {
    return this.http.get<ISelectInfo[]>(environment.apiUrl +  'MedicalInstituation/autocomplete');
  }

  getDoctorByMedicalInstitution(id: number): Observable<ISelectInfo[]> {
    return this.http.get<ISelectInfo[]>(environment.apiUrl +  'Doctor/doctors-by-instituation-autocomplete/' + id);
  }

  createRecord(form): Observable<any> {
    return this.http.post(environment.apiUrl +  'Record', form);
  }

  getPatientRecords(): Observable<IRecord[]> {
    return this.http.get<IRecord[]>(environment.apiUrl +  'Record/patient-records');
  }

  getPatientRecordsById(patientId: number): Observable<IRecord[]> {
    return this.http.get<IRecord[]>(environment.apiUrl +  'Record/patient-records-by-id/' + patientId);
  }

  getDoctorRecords(): Observable<IRecord[]> {
    return this.http.get<IRecord[]>(environment.apiUrl +  'Record/doctor-records');
  }

  cancelRecordLikePatient(status: any, recordId: number): Observable<any> {
    return this.http.put(environment.apiUrl +  'Record/update-status-like-patient?status=' + status + '&recordId=' + recordId, {});
  }

  updateRecord(form): Observable<any> {
    return this.http.put(environment.apiUrl +  'Record/update-record', form);
  }

  updateRecordLikeDoctor(status: any, recordId: number): Observable<any> {
    return this.http.put(environment.apiUrl +  'Record/update-status-like-doctor?status=' + status + '&recordId=' + recordId, {});
  }

  getPatientInfo(patientId: number): Observable<IPatient> {
    return this.http.get<IPatient>(environment.apiUrl +  'Patient/get-by-id/' + patientId);
  }

  getDiseaseHistoryInfo(patientId: number): Observable<IDiseaseHistory> {
    return this.http.get<IDiseaseHistory>(environment.apiUrl +  'DiseaseHistory/disease-history-by-patient-id/' + patientId);
  }

  updateDiseaseHistory(form: IDiseaseHistory): Observable<any> {
    return this.http.put(environment.apiUrl +  'DiseaseHistory', form);
  }

  createDiseaseHistory(form: IDiseaseHistory): Observable<any> {
    return this.http.post(environment.apiUrl +  'DiseaseHistory', form);
  }
}
