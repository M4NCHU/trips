import { GeoLocation } from "./GeoLocation/GeoLocation";
import { Category } from "./Category";
import { VisitPlace } from "./VisitPlaceTypes";

export type Accommodation = {
  id: string;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
  price: number;
  categoryId: string;
  visitPlaces: VisitPlace[];
  geoLocation: GeoLocation;
};

export type CreateDestination = {
  name: string;
  description: string;
  location?: string;
  photoUrl?: string;
  price: number;
  categoryId: string;
};

export type AccommodationCategory = Accommodation & { category: Category };
