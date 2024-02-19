import { Category } from "./Category";
import { VisitPlace } from "./VisitPlaceTypes";

export type Destination = {
  id: string;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
  price: number;
  categoryId: string;
  visitPlaces: VisitPlace[];
};

export type DestinationCategory = Destination & { category: Category };
