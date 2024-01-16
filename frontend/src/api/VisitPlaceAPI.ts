// api/visitPlaces.timport { useQuery } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import { usePagination } from "../hooks/usePagination";
import { VisitPlace } from "../types/VisitPlaceTypes";
import { fetchData } from "./apiUtils";

// // Get visitPlace by id
// export const GetVisitPlaces = () => {
//   return useQuery<VisitPlace, Error>({
//     queryKey: ["visitPlace"],
//     queryFn: async () => {
//       return fetchData<VisitPlace>(`/api/VisitPlace`);
//     },
//   });
// };

// Adding visitPlace
export const createVisitPlace = async (formData: FormData) => {
  try {
    const response = await fetchData<VisitPlace>("/api/VisitPlace", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating visit place:", error);
    throw new Error("Failed to create visit place. Please try again.");
  }
};

// Get visitPlace by destination id
export const GetVisitPlacesByDestination = (id: string) => {
  return useQuery<VisitPlace[], Error>({
    queryKey: ["visitPlaceByDestinationId"],
    queryFn: async () => {
      return fetchData<VisitPlace[]>(`/api/VisitPlace/destination/${id}`);
    },
  });
};

// Get visitPlace by destination id
export const GetVisitPlacesById = (id: string) => {
  return useQuery<VisitPlace[], Error>({
    queryKey: ["visitPlaceByDestinationId"],
    queryFn: async () => {
      return fetchData<VisitPlace[]>(`/api/VisitPlace/${id}`);
    },
  });
};
