import { TripDestination } from "./TripDestinationTypes";
import { VisitPlace } from "./VisitPlaceTypes";

export type Accommodation = {
  id: number;
  tripDestinationId: number;
  tripDestination: TripDestination;
  visitPlaceId: number;
  visitPlace: VisitPlace;
};
