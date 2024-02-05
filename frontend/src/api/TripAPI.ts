// api/trips.timport { useQuery } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";

// Adding trip
export const UseCreateTrip = async (formData: FormData) => {
  try {
    const response = await fetchData<Trip>("/api/Trip", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating trip:", error);
    throw new Error("Failed to create trip. Please try again.");
  }
};

// Get trip by id
export const UseTripById = (id: string | undefined) => {
  return useQuery<Trip, Error>({
    queryKey: ["tripById"],
    queryFn: async () => {
      return fetchData<Trip>(`/api/Trip/${id}`);
    },
    enabled: !!id,
  });
};

// Get all trips for user
export const UseUserTripsList = (userId: string | undefined) => {
  return useQuery<Trip[], Error>({
    queryKey: ["userTripsList"],
    queryFn: async () => {
      return fetchData<Trip[]>(`/api/Trip/UserId/${userId}`);
    },
    enabled: !!userId,
  });
};

// Get user trips count
export const UseUserTripsCount = (
  userId: string | undefined,
  status: number
) => {
  return useQuery<number, Error>({
    queryKey: ["userTripsCount"],
    queryFn: async () => {
      return fetchData<number>(
        `/api/Trip/count/userId/${userId}/status/${status}`
      );
    },
    enabled: !!userId,
  });
};

// Change trip title
export const UseChangeTripTitle = async (tripId: string, newTitle: string) => {
  try {
    // Construct the endpoint with the tripId
    const endpoint = `/api/Trip/${tripId}/Title/${newTitle}`;

    // Prepare the request body
    const requestBody = {
      newTitle: newTitle,
    };

    const response = await fetchData<Trip>(endpoint, {
      method: "patch",
    });

    return response;
  } catch (error) {
    console.error("Error changing trip title:", error);
    throw new Error("Failed to change trip title. Please try again.");
  }
};
