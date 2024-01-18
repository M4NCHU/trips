export type Participant = {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
  emergencyContactName: string;
  emergencyContactPhone: string;
  medicalConditions: string;
  createdAt: string;
  modifiedAt?: string | null;
  photoUrl: string;
  imageFile?: File | null;
  tripId: number;
};
