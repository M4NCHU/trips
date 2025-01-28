import { CartItemType } from "./enums/CartItemType";

export interface ReservationItemModel {
  id: string;
  reservationId: string;
  itemType: CartItemType;
  itemId: string;
  price: number;
  quantity: number;
  additionalData?: string;
  createdAt: string;
}
