import { Participant } from "./ParticipantTypes";
import { Trip } from "./TripTypes";

export type TripParticipant = {
  id: number;
  tripId: number;
  trip: Trip;
  ParticipantId: number;
  participants: Participant;
};
