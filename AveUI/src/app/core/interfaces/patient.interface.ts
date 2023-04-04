import { IConfidant } from './confidant.interface';
import { IDiseaseHistory } from './disease-history.interface';
import { IRecord } from './record.interface';

export interface IPatient {
  id: number;
  userId: string;
  firstName: string;
  lastName: string;
  country: string;
  passport: string;
  address: string;
  diseaseHistoryId: number | null;
  phone: string;
  records: IRecord[];
  confidants: IConfidant[];
  diseaseHistory: IDiseaseHistory;
}

