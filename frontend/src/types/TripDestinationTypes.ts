import { Trip } from "./TripTypes";
import { Destination } from "./Destination";
import { SelectedPlace } from "./SelectedPlaceTypes";

export type TripDestination = {
  id: number;
  tripId: number;
  trip: Trip;
  destinationId: number;
  destination: Destination;
  selectedPlaces: SelectedPlace[];
};
