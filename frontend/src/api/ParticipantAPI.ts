import { useMutation, useQuery } from "@tanstack/react-query";
import { Participant } from "../types/ParticipantTypes";
import { fetchData } from "./apiUtils";
import toast from "react-hot-toast";

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

export const useParticipantById = (id: string) => {
  return useQuery<Participant, Error>({
    queryKey: ["ParticipantById", id],
    queryFn: async () => {
      return fetchData<Participant>(`/api/Participant/${id}`);
    },
  });
};

export const UseCreateParticipant = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<Participant>("/api/Participant", {
          method: "post",
          data: formData,
        });
        toast.success("Participant created successfully!");
        return response;
      } catch (error: any) {
        toast.error(error.message || "An unexpected error occurred.");
        throw new Error("Failed to create destination. Please try again.");
      }
    },
  });
  return mutation;
};
