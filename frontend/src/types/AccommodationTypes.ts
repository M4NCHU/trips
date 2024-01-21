import { TripDestination } from "./TripDestinationTypes";
import { VisitPlace } from "./VisitPlaceTypes";

export type Accommodation = {
  id: string;
  tripDestinationId: string;
  tripDestination: TripDestination;
  visitPlaceId: string;
  visitPlace: VisitPlace;
};
