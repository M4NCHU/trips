import { useMutation, useQuery } from "@tanstack/react-query";
import { CreateVisitPlace, VisitPlace } from "../types/VisitPlaceTypes";
import { fetchData } from "./apiUtils";

export const UseCreateVisitPlace = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<CreateVisitPlace>("/api/VisitPlace", {
          method: "post",
          data: formData,
        });

        return response;
      } catch (error) {
        console.error("Error creating visit place:", error);
        throw new Error("Failed to create visit place. Please try again.");
      }
    },
    onSuccess: () => {},
  });
  return mutation;
};

export const useVisitPlacesByDestination = (id: string | undefined) => {
  return useQuery<VisitPlace[], Error>({
    queryKey: ["visitPlaceByDestination", id],
    queryFn: async () => {
      if (!id) {
        throw new Error("No ID provided");
      }
      return fetchData<VisitPlace[]>(`/api/VisitPlace/destination/${id}`);
    },
    enabled: !!id,
  });
};

export const useVisitPlacesById = (id: string | undefined) => {
  return useQuery<VisitPlace, Error>({
    queryKey: ["visitPlaceById", id],
    queryFn: async () => {
      if (!id) {
        throw new Error("No ID provided");
      }
      return fetchData<VisitPlace>(`/api/VisitPlace/${id}`);
    },
    enabled: !!id,
  });
};
