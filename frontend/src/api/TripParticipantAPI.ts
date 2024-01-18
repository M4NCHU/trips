import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { TripParticipant } from "../types/TripParticipantTypes";
import { fetchData } from "./apiUtils";

// Get all trip participants
export const useTripParticipants = (page = 1, pageSize = 2) => {
  return useQuery<TripParticipant[], Error>({
    queryKey: ["tripParticipants", page, pageSize],
    queryFn: async () => {
      return fetchData<TripParticipant[]>(
        `/api/TripParticipant?page=${page}&pageSize=${pageSize}`
      );
    },
  });
};

// Get trip participant by id
export const useTripParticipantById = (id: string) => {
  return useQuery<TripParticipant, Error>({
    queryKey: ["tripParticipantById", id],
    queryFn: async () => {
      return fetchData<TripParticipant>(`/api/TripParticipant/${id}`);
    },
  });
};

// Update trip participant
// export const useUpdateTripParticipant = () => {
//   const queryClient = useQueryClient();

//   return useMutation<void, Error, { id: number; tripParticipant: TripParticipant }>(
//     ({ id, tripParticipant }) => fetchData<void>(`/api/TripParticipant/${id}`, {
//       method: "put",
//       data: tripParticipant,
//     }),
//     {
//       onSuccess: () => {
//         // Invalidate and refetch the trip participants query
//         queryClient.invalidateQueries("tripParticipants");
//       },
//     }
//   );
// };

// Create trip participant
export const UseCreateTripParticipant = async (formData: FormData) => {
  try {
    const response = await fetchData<TripParticipant>("/api/TripParticipant", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating destination:", error);
    throw new Error("Failed to create destination. Please try again.");
  }
};

// Delete trip participant
// export const useDeleteTripParticipant = () => {
//   const queryClient = useQueryClient();

//   return useMutation<void, Error, number>(
//     (id) => fetchData<void>(`/api/TripParticipant/${id}`, {
//       method: "delete",
//     }),
//     {
//       onSuccess: () => {
//         // Invalidate and refetch the trip participants query
//         queryClient.invalidateQueries("tripParticipants");
//       },
//     }
//   );
// };
