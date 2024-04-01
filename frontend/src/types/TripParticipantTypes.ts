import { Participant } from "./ParticipantTypes";
import { Trip } from "./TripTypes";

export type TripParticipant = {
  id: string;
  tripId: string;
  trip: Trip;
  ParticipantId: string;
  participants: Participant;
};

export type CreateTripParticipant = {
  tripId: string;
  ParticipantId: string;
};
