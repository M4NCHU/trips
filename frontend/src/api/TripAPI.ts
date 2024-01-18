// api/trips.timport { useQuery } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";

// Adding trip
export const createTrip = async (formData: FormData) => {
  try {
    const response = await fetchData<Trip>("/api/Trip/CreateTrip", {
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
export const GetTripById = (id: string) => {
  return useQuery<Trip, Error>({
    queryKey: ["tripById"],
    queryFn: async () => {
      return fetchData<Trip>(`/api/Trip/GetTripById/${id}`);
    },
  });
};
