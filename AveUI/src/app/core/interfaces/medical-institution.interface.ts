import { ISelectInfo } from './../../shared/components/controls-test/controls-test.component';

export interface IMedicalInstitution {
  id: number;
  name: string;
  country: string;
  city: string;
  address: string;
  doctors: ISelectInfo[];
}
