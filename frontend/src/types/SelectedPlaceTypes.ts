import { TripDestination } from "./TripDestinationTypes";
import { VisitPlace } from "./VisitPlaceTypes";

export type SelectedPlace = {
  id: string;
  tripDestinationId: string;
  tripDestination: TripDestination;
  visitPlaceId: string;
  visitPlace: VisitPlace;
};
