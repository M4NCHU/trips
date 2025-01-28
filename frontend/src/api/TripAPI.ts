import { useMutation, useQuery } from "@tanstack/react-query";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";

export const UseCreateTrip = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<{
          title: string;
          createdBy: boolean;
          tripId: string;
        }>("/api/Trip", {
          method: "post",
          data: formData,
        });

        return response;
      } catch (error: any) {
        throw new Error("Failed to create destination. Please try again.");
      }
    },
  });
  return mutation;
};

export const UseTripById = (id: string | undefined) => {
  return useQuery<Trip, Error>({
    queryKey: ["tripById"],
    queryFn: async () => {
      return fetchData<Trip>(`/api/Trip/${id}`);
    },
    enabled: !!id,
  });
};

export const UseUserTripsList = (userId: string | undefined) => {
  return useQuery<Trip[], Error>({
    queryKey: ["userTripsList"],
    queryFn: async () => {
      return fetchData<Trip[]>(`/api/Trip/UserId/${userId}`);
    },
    enabled: !!userId,
  });
};

export const UseEnsureActiveTripExists = (userId: string | undefined) => {
  return useQuery<{ tripId: string; wasTripCreated: boolean }, Error>({
    queryKey: ["ensureActiveTripExists", userId],
    queryFn: async () => {
      const response = await fetch(`/api/Trip/ensure/UserId/${userId}`);
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    },
    enabled: !!userId,
  });
};

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

export const UseChangeTripTitle = async (tripId: string, newTitle: string) => {
  try {
    const endpoint = `/api/Trip/${tripId}/Title/${newTitle}`;
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
