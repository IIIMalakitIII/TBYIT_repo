import { IDoctor } from './doctor.interface';
import { IPatient } from './patient.interface';

export interface IAccountInfo {
  userName: string;
  email: string;
  patient: IPatient;
  doctor: IDoctor;
}
