import { Destination } from "../Destination";

export type CartItem = {
  id: string;
  userId: string;
  itemType: number;
  itemId: string;
  quantity: number;
  additionalData: string;
  createdAt: string;
  modifiedAt: string;
  destination?: Destination | null;
};
