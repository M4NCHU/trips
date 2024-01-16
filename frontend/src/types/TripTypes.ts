import { TripDestination } from "./TripDestinationTypes";
import { SelectedPlace } from "./SelectedPlaceTypes";
import { TripStatus } from "./enums/TripStatusEnum";
import { Destination } from "./Destination";
import { VisitPlace } from "./VisitPlaceTypes";

export type Trip = {
  id: number;
  status: TripStatus;
  startDate?: Date | null;
  endDate?: Date | null;
  tripDestinations: TripDestination[];
  totalPrice: number;
};
