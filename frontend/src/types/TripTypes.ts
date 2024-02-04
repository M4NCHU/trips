import { TripDestination } from "./TripDestinationTypes";
import { TripStatus } from "./enums/TripStatusEnum";
import { UserDTOTypes } from "./user/UserDTOTypes";

export type Trip = {
  id: string;
  status: TripStatus;
  startDate?: Date | null;
  endDate?: Date | null;
  tripDestinations: TripDestination[];
  totalPrice: number;
  createdBy: string;
  user: UserDTOTypes | null;
  title: string;
};
