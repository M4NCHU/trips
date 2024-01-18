import { Destination, DestinationCategory } from "./Destination";
import { SelectedPlace } from "./SelectedPlaceTypes";
import { Trip } from "./TripTypes";

export type TripDestination = {
  id: number;
  tripId: number;
  trip: Trip;
  destinationId: number;
  destination: Destination;
  selectedPlaces: SelectedPlace[];
};

export type TripDestinationInterface = {
  tripId: number;
  destinationId: number;
  destination: DestinationCategory;
  selectedPlaces: SelectedPlace[];
};
