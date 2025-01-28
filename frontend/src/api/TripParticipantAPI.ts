import { useMutation, useQuery } from "@tanstack/react-query";
import {
  CreateTripParticipant,
  TripParticipant,
} from "../types/TripParticipantTypes";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";
import toast from "react-hot-toast";

export const UseCreateTripParticipant = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<CreateTripParticipant>(
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
