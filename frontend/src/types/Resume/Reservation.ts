import { ReservationStatus } from "./enums/ReservationStatus";
import { ReservationItemModel } from "./ReservationItem";

export interface Reservation {
  id: string;
  userId: string;
  reservationItems: ReservationItemModel[];
  status: ReservationStatus;
  totalPrice: number;
  createdAt: string;
  modifiedAt?: string;
}
