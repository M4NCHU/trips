import { ReservationItemDTO } from "./ReservationItemDTO";

export interface ReservationDTO {
  id: string;
  userId: string | null;
  totalPrice: number;
  createdAt: string;
  status?: string;
  reservationItems: ReservationItemDTO[];
}
