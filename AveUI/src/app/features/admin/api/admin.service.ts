import { ISelectInfo } from './../../../shared/components/controls-test/controls-test.component';
import { IPatient } from '../../../core/interfaces/patient.interface';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IRecord } from 'src/app/core/interfaces/record.interface';
import { environment } from 'src/environments/environment';
import { IDiseaseHistory } from 'src/app/core/interfaces/disease-history.interface';
import { IMedicalInstitution } from 'src/app/core/interfaces/medical-Institution.interface';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

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

  getRecordById(id: number): Observable<IRecord> {
    return this.http.get<IRecord>(environment.apiUrl +  'Record/get-record-by-id/' + id);
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

  getMedicamentCategories(): Observable<any[]> {
    return this.http.get<any[]>(environment.apiUrl +  'MedicamentCategory');
  }

  updateReceiptStatus(receiptId: number, status: any): Observable<any> {
    return this.http.put(environment.apiUrl +  'Receipt/update-receipt-status?status=' + status + '&id=' + receiptId, {});
  }

  getManufacturers(): Observable<ISelectInfo[]> {
    return this.http.get<ISelectInfo[]>(environment.apiUrl +  'Manufacturer/autocomplete');
  }

  createManufacture(form: any): Observable<any> {
    return this.http.post(environment.apiUrl +  'Medicament', form);
  }

  updateManufacture(form: any): Observable<any> {
    return this.http.put(environment.apiUrl +  'Medicament', form);
  }

  createMedicamentCategory(form): Observable<any> {
    return this.http.post(environment.apiUrl +  'MedicamentCategory', form);
  }

  updateMedicamentCategory(form): Observable<any> {
    return this.http.put(environment.apiUrl +  'MedicamentCategory', form);
  }

  createManufacturer(form): Observable<any> {
    return this.http.post(environment.apiUrl +  'Manufacturer', form);
  }

  updateManufacturer(form): Observable<any> {
    return this.http.put(environment.apiUrl +  'Manufacturer', form);
  }

  createMedicalInstituation(form): Observable<any> {
    return this.http.post(environment.apiUrl +  'MedicalInstituation', form);
  }

  getAllMedicalInstitutions(): Observable<IMedicalInstitution[]> {
    return this.http.get<IMedicalInstitution[]>(environment.apiUrl +  'MedicalInstituation');
  }


  updateMedicalInstituatio(form): Observable<any> {
    return this.http.put(environment.apiUrl +  'MedicalInstituation', form);
  }

  exportDataAsExcel(): Observable<any> {
    return this.http.get(environment.apiUrl + 'Account/export-data', { responseType: 'text' });
  }

}
