import { useQuery } from "@tanstack/react-query";
import { TripParticipant } from "../types/TripParticipantTypes";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";
import toast from "react-hot-toast";

// Adding trip participant
export const UseCreateTripParticipant = async (
  tripId: string,
  participantId: string
) => {
  try {
    const response = await fetchData<TripParticipant>("/api/TripParticipant", {
      method: "post",
      data: {
        tripId: tripId,
        participantId: participantId,
      },
    });
    toast.success("Trip participant created successfully!");
    return response;
  } catch (error: any) {
    toast.error(error.message || "An unexpected error occurred.");
    throw new Error("Failed to create trip participant. Please try again.");
  }
};

// Get trip participant by id
export const UseTripParticipant = (tripId: string | undefined) => {
  return useQuery<TripParticipant[], Error>({
    queryKey: ["tripParticipantsByTripId", tripId],
    queryFn: async () => {
      return fetchData<TripParticipant[]>(
        `/api/TripParticipant/trip/${tripId}`
      );
    },
    enabled: !!tripId,
  });
};
