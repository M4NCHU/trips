import { Category } from "./Category";

export type Destination = {
  id: number;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
  categoryId: number;
};

export type DestinationCategory = Destination & { category: Category };
