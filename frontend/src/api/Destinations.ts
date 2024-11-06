// api/destinations.timport { useQuery } from "@tanstack/react-query";
import { useMutation, useQuery } from "@tanstack/react-query";
import { usePagination } from "../hooks/usePagination";
import {
  CreateDestination,
  Destination,
  DestinationCategory,
} from "../types/Destination";
import { fetchData } from "./apiUtils";
import { DestinationValidator } from "../lib/validators/DestinationValidator";
import toast from "react-hot-toast";
import { ZodError, z } from "zod";

export interface DestinationFilter {
  categoryId?: string;
}

// Get destinations with pagination

export const useDestinationList = (filter?: any, pageSize = 20) => {
  const {
    isLoading,
    isError,
    error,
    data,
    totalItems,
    currentPage,
    isFetching,
    page,
    setPage,
  } = usePagination<Destination>("/api/Destination", {
    pageSize: pageSize,
    queryParameters: filter,
  });

  return {
    isLoading,
    isError,
    error,
    data,
    totalItems,
    currentPage,
    isFetching,
    page,
    setPage,
  };
};

// Get destination by id
export const useDestinationById = (id: string | undefined) => {
  return useQuery<DestinationCategory, Error>({
    queryKey: ["destination", id],
    queryFn: () => {
      if (!id) {
        throw new Error("No ID provided");
      }
      return fetchData<DestinationCategory>(`/api/Destination/${id}`);
    },
    enabled: !!id,
  });
};

export const useSearchDestinations = (searchTerm: string) => {
  return useQuery<Destination[], Error>({
    queryKey: ["searchDestinations", searchTerm],
    queryFn: () =>
      fetchData<Destination[]>(
        `/api/Destination/Search?searchTerm=${encodeURIComponent(searchTerm)}`
      ),
    enabled: !!searchTerm,
  });
};

// Adding destination
export const UseCreateDestination = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<CreateDestination>(
          "/api/Destination",
          {
            method: "post",
            data: formData,
          }
        );
        return response;
      } catch (error: any) {
        throw new Error("Failed to create destination. Please try again.");
      }
    },
  });
  return mutation;
};
