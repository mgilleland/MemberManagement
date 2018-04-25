export interface IMember {
  id: number;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  dateOfBirth: Date;
}
