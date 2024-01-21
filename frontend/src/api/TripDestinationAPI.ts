import { useQuery } from "@tanstack/react-query";
import { TripDestination } from "../types/TripDestinationTypes";
import { fetchData } from "./apiUtils";

// Adding trip participant
export const UseCreateTripDestination = async (formData: FormData) => {
  try {
    const response = await fetchData<TripDestination>("/api/TripDestination", {
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
export const UseTripDestination = (id: string) => {
  return useQuery<TripDestination, Error>({
    queryKey: ["ByTripDestinationId"],
    queryFn: async () => {
      return fetchData<TripDestination>(`/api/TripDestination/${id}`);
    },
  });
};

// Get trip destinations count
export const UseTripDestinationCount = (tripId: string) => {
  return useQuery<number>({
    queryKey: ["ByTripDestinationCount"],
    queryFn: async () => {
      return fetchData<number>(
        `/api/TripDestination/trip-destination-count/${tripId}`
      );
    },
  });
};
