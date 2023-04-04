import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IArtificialOrgan } from 'src/app/core/interfaces/artificial-organ.interface';
import { IConnectHistory } from 'src/app/core/interfaces/connect-history';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ArtificialOrganService {

  constructor(private http: HttpClient) {}

  getPatientArtificialOrgans(id: number): Observable<IArtificialOrgan[]> {
    return this.http.get<IArtificialOrgan[]>(environment.apiUrl +  'ArtificialOrgan/patient-organ/' + id);
  }

  getPatientOrgan(id: number): Observable<IArtificialOrgan> {
    return this.http.get<IArtificialOrgan>(environment.apiUrl +  'ArtificialOrgan?id=' + id);
  }

  addArtificialOrgan(form): Observable<any> {
    return this.http.post<any>(environment.apiUrl +  'ArtificialOrgan', form);
  }

  generateKeys(organId): Observable<any> {
    return this.http.post<any>(environment.apiUrl +  'ArtificialOrgan/generate-organ-key/' + organId, {});
  }

  diagnosticOrgan(organId, from: string, to: string): Observable<string> {
    return this.http.get<string>(environment.apiUrl +  'ArtificialOrgan/diagnostic-organ/' + organId + '/' + from + '/' + to);
  }

  getConnectHistory(organId): Observable<IConnectHistory[]> {
    return this.http.get<IConnectHistory[]>(environment.apiUrl +  'ArtificialOrgan/connect-history/' + organId);
  }

}
