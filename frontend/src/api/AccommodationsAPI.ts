import { useMutation, useQuery } from "@tanstack/react-query";
import { usePagination } from "../hooks/usePagination";

import { fetchData } from "./apiUtils";
import toast from "react-hot-toast";
import { ZodError, z } from "zod";
import { CreateDestination } from "src/types/Destination";
import { Accommodation } from "src/types/Accommodation";

export interface AccommodationFilter {
  categoryId?: string;
}

export const useAccommodationList = (filter?: any, pageSize = 20) => {
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
  } = usePagination<Accommodation>("/api/Accommodations", {
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

export const useSearchAccommodations = (searchTerm: string) => {
  return useQuery<Accommodation[], Error>({
    queryKey: ["searchAccommodations", searchTerm],
    queryFn: () =>
      fetchData<Accommodation[]>(
        `/api/Accommodations/Search?searchTerm=${encodeURIComponent(
          searchTerm
        )}`
      ),
    enabled: !!searchTerm,
  });
};

export const UseCreateAccommodation = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        const response = await fetchData<CreateDestination>(
          "/api/Accommodations",
          {
            method: "post",
            data: formData,
          },
          true
        );
        return response;
      } catch (error: any) {
        throw new Error("Failed to create accommodation. Please try again.");
      }
    },
  });
  return mutation;
};
