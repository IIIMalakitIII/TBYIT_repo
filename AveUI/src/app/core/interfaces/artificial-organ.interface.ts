import { IArtificialOrganKey } from "./artificial-organ-key.interface";
import { IAuditableEntity } from "./auditable-entity.interface";
import { IOrganData } from "./organ-data.interface";
import { IPatient } from "./patient.interface";

export interface IArtificialOrgan extends IAuditableEntity {
  id: number;
  name: string;
  description: string;
  organType: string;
  patientId: number;
  patient: IPatient;
  artificialOrganKeys: IArtificialOrganKey[];
  organDatas: IOrganData[];
}
