// api/destinations.timport { useQuery } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import { usePagination } from "../hooks/usePagination";
import { Destination, DestinationCategory } from "../types/Destination";
import { fetchData } from "./apiUtils";
import { DestinationValidator } from "../lib/validators/destination";
import toast from "react-hot-toast";
import { ZodError, z } from "zod";

// Get destinations with pagination
export const useDestinationList = () => {
  const {
    isPending,
    isError,
    error,
    data,
    isFetching,
    isPlaceholderData,
    fetchNextPage,
    fetchPreviousPage,
    page,
    setPage,
  } = usePagination<Destination>("/api/Destination", {
    pageSize: 2,
  });

  return {
    isPending,
    isError,
    error,
    data,
    isFetching,
    isPlaceholderData,
    fetchNextPage,
    fetchPreviousPage,
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

// Adding destination
export const UseCreateDestination = async (formData: FormData) => {
  try {
    const response = await fetchData<Destination>("/api/Destination", {
      method: "post",
      data: formData,
    });

    toast.success("Destination created successfully!");
    return response;
  } catch (error: any) {
    toast.error(error.message || "An unexpected error occurred.");
    throw new Error("Failed to create destination. Please try again.");
  }
};
