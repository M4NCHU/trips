// api/destinations.timport { useQuery } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import { usePagination } from "../hooks/usePagination";
import { Destination, DestinationCategory } from "../types/Destination";
import { fetchData } from "./apiUtils";

// Get destinations with pagination
export const GetDestinationList = () => {
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
  } = usePagination<Destination>("/api/Destinations", { pageSize: 8 });

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
export const GetDestinationById = (id: string) => {
  return useQuery<DestinationCategory, Error>({
    queryKey: ["destination"],
    queryFn: async () => {
      return fetchData<DestinationCategory>(`/api/Destinations/${id}`);
    },
  });
};

// Adding destination
export const createDestination = async (formData: FormData) => {
  console.log(formData);
  try {
    const response = await fetchData<Destination>("/api/Destinations", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating destination:", error);
    throw new Error("Failed to create destination. Please try again.");
  }
};
