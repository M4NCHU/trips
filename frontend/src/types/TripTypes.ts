import { TripDestination } from "./TripDestinationTypes";
import { TripStatus } from "./enums/TripStatusEnum";

export type Trip = {
  id: number;
  status: TripStatus;
  startDate?: Date | null;
  endDate?: Date | null;
  tripDestinations: TripDestination[];
  totalPrice: number;
};
