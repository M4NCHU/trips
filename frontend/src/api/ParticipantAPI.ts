import { useQuery } from "@tanstack/react-query";
import { Participant } from "../types/ParticipantTypes";
import { fetchData } from "./apiUtils";

// Get all  participants
export const useParticipants = (page = 1, pageSize = 2) => {
  return useQuery<Participant[], Error>({
    queryKey: ["Participants", page, pageSize],
    queryFn: async () => {
      return fetchData<Participant[]>(
        `/api/Participant?page=${page}&pageSize=${pageSize}`
      );
    },
  });
};

// Get participant by id
export const useParticipantById = (id: string) => {
  return useQuery<Participant, Error>({
    queryKey: ["ParticipantById", id],
    queryFn: async () => {
      return fetchData<Participant>(`/api/Participant/${id}`);
    },
  });
};

// Update participant
// export const useUpdateParticipant = () => {
//   const queryClient = useQueryClient();

//   return useMutation<void, Error, { id: number; Participant: Participant }>(
//     ({ id, Participant }) => fetchData<void>(`/api/Participant/${id}`, {
//       method: "put",
//       data: Participant,
//     }),
//     {
//       onSuccess: () => {
//         // Invalidate and refetch the  participants query
//         queryClient.invalidateQueries("Participants");
//       },
//     }
//   );
// };

// Create  participant
export const UseCreateParticipant = async (formData: FormData) => {
  try {
    const response = await fetchData<Participant>("/api/Participant", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating destination:", error);
    throw new Error("Failed to create destination. Please try again.");
  }
};

// Delete  participant
// export const useDeleteParticipant = () => {
//   const queryClient = useQueryClient();

//   return useMutation<void, Error, number>(
//     (id) => fetchData<void>(`/api/Participant/${id}`, {
//       method: "delete",
//     }),
//     {
//       onSuccess: () => {
//         // Invalidate and refetch the  participants query
//         queryClient.invalidateQueries("Participants");
//       },
//     }
//   );
// };
