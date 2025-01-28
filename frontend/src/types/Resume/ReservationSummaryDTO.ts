import { ReservationItemDTO } from "./ReservationItemDTO";

export interface ReservationSummaryDTO {
  ReservationId: string;
  UserId: string | null;
  TotalPrice: number;
  CreatedAt: string;
  Items: ReservationItemDTO[];
}
