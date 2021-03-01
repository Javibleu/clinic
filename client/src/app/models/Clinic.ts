import { User } from './user';

export interface Clinic {
  id: number;
  clinicName: string;
  phone: string;
  address: string;
  url: string;
  email: string;
  card: string;
  ownBy: User;
  healthProviderType: number;
 }
