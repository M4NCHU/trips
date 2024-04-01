import { Destination, DestinationCategory } from "./Destination";
import { SelectedPlace } from "./SelectedPlaceTypes";
import { Trip } from "./TripTypes";

export type TripDestination = {
  id: string;
  tripId: string;
  trip: Trip;
  destinationId: string;
  destination: Destination;
  selectedPlaces: SelectedPlace[];
};

export type TripDestinationInterface = {
  id: string;
  tripId: string;
  destinationId: string;
  destination: DestinationCategory;
  selectedPlaces: SelectedPlace[];
};

export type CreateTripDestination = {
  tripId: string;
  destinationId: string;
  selectedPlaces: SelectedPlace[];
};
