export type Participant = {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
  emergencyContactName: string;
  emergencyContact: string;
  medicalConditions: string;
  createdAt: string;
  modifiedAt?: string | null;
  photoUrl: string;
  imageFile?: File | null;
  tripId: string;
};
