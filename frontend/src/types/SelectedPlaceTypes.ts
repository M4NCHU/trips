import { TripDestination } from "./TripDestinationTypes";
import { VisitPlace } from "./VisitPlaceTypes";

export type SelectedPlace = {
  id: number;
  tripDestinationId: number;
  tripDestination: TripDestination;
  visitPlaceId: number;
  visitPlace: VisitPlace;
};
