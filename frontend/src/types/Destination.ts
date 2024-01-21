import { Category } from "./Category";

export type Destination = {
  id: string;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
  price: number;
  categoryId: string;
};

export type DestinationCategory = Destination & { category: Category };
