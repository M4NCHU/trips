import { useQuery } from "@tanstack/react-query";
import { TripParticipant } from "../types/TripParticipantTypes";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";

// Adding trip participant
export const createTrip = async (formData: FormData) => {
  try {
    const response = await fetchData<Trip>("/api/TripParticipant", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating trip:", error);
    throw new Error("Failed to create trip. Please try again.");
  }
};

// Get trip participant by id
export const GetTripParticipant = (id: string) => {
  return useQuery<TripParticipant, Error>({
    queryKey: ["tripParticipantsByTripId"],
    queryFn: async () => {
      return fetchData<TripParticipant>(
        `/api/TripParticipant/GetTripParticipant/${id}`
      );
    },
  });
};
