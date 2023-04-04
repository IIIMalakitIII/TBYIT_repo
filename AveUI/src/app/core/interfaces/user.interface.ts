import { IDoctor } from 'src/app/core/interfaces/doctor.interface';
import { IPatient } from './patient.interface';
export interface IUser {
  token?: string;
  er_role: string;
  name: string;
  id?: string;
  user_id?: string;
}

export interface IUserInfo {
  email: string;
  roles: any;
  userName: string;
  id: number;
}

export interface IFullUserInfo {
  id: string;
  userName: string;
  email: string;
  patient?: IPatient;
  doctor?: IDoctor;
}
