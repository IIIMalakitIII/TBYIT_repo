import { IArtificialOrgan } from "./artificial-organ.interface";
import { IAuditableEntity } from "./auditable-entity.interface";

export interface IArtificialOrganKey extends IAuditableEntity {
  id: number;
  artificialOrganId: number;
  accessKey: string;
  artificialOrgan: IArtificialOrgan;
}
