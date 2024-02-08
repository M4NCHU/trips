import { useMutation, useQuery } from "@tanstack/react-query";
import { TripParticipant } from "../types/TripParticipantTypes";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";
import toast from "react-hot-toast";

// Adding trip participant
export const UseCreateTripParticipant = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<TripParticipant>(
          "/api/TripParticipant",
          {
            method: "post",
            data: {
              formData,
            },
          }
        );
        return response;
      } catch (error: any) {
        throw new Error("Failed to create trip participant. Please try again.");
      }
    },
  });
  return mutation;
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
