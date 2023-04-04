import { ISelectInfo } from "./select-info.interface";

export interface IConfidant {
  id: number;
  patientId: number;
  doctorId: number;
  patient: ISelectInfo;
  doctor: ISelectInfo;
}
