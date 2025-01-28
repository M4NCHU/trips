export interface ReservationDetailsModel {
  id: string;
  reservationId: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  address: string;
  city?: string;
  country?: string;
  postalCode?: string;
  additionalNotes?: string;
  createdAt: string;
  modifiedAt?: string;
}
