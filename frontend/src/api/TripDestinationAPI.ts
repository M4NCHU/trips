import { useMutation, useQuery } from "@tanstack/react-query";
import {
  CreateTripDestination,
  TripDestination,
} from "../types/TripDestinationTypes";
import { fetchData } from "./apiUtils";

export const UseCreateTripDestination = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<CreateTripDestination>(
          "/api/TripDestination",
          {
            method: "post",
            data: formData,
          }
        );

        return response;
      } catch (error: any) {
        throw new Error("Failed to add trip destionation. Please try again.");
      }
    },
  });
  return mutation;
};

export const UseTripDestination = (id: string) => {
  return useQuery<TripDestination, Error>({
    queryKey: ["ByTripDestinationId"],
    queryFn: async () => {
      return fetchData<TripDestination>(`/api/TripDestination/${id}`);
    },
  });
};

export const UseTripDestinationByTripId = (id: string | undefined) => {
  return useQuery<TripDestination, Error>({
    queryKey: ["ByTripDestinationId"],
    queryFn: async () => {
      return fetchData<TripDestination>(`/api/TripDestination/trip/${id}`);
    },
    enabled: !!id,
  });
};

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
