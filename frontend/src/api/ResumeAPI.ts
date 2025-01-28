import { useQuery, useMutation } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { fetchData } from "./apiUtils";
import { ReservationSummaryDTO } from "src/types/Resume/ReservationSummaryDTO";
import { ReservationDTO } from "src/types/Resume/ReservationDTO";

export const useGetReservationSummary = (reservationId: string) => {
  return useQuery<ReservationSummaryDTO, Error>({
    queryKey: ["reservationSummary", reservationId],
    queryFn: async () => {
      const response = await fetchData<{ result: ReservationSummaryDTO }>(
        `/api/Reservation/${reservationId}/summary`
      );
      return response.result;
    },
    enabled: !!reservationId,
  });
};

export const useCreateReservation = () => {
  return useMutation({
    mutationFn: async (cartCookie: string) => {
      try {
        const response = await fetchData<ReservationDTO>(
          "/api/Reservation/create",
          {
            method: "post",
            data: { cartCookie },
          }
        );
        toast.success("Reservation created successfully!");
        return response;
      } catch (error: any) {
        toast.error("Failed to create reservation. Please try again.");
        throw new Error(error.message || "An error occurred.");
      }
    },
  });
};

export const useGetUserReservations = () => {
  return useQuery<ReservationDTO[], Error>({
    queryKey: ["userReservations"],
    queryFn: async () => {
      return fetchData<ReservationDTO[]>(`/api/Reservation/user-reservations`);
    },
  });
};

export const useGetReservationById = (reservationId: string) => {
  return useQuery<ReservationDTO, Error>({
    queryKey: ["reservationById", reservationId],
    queryFn: async () => {
      return await fetchData<ReservationDTO>(
        `/api/Reservation/${reservationId}`
      );
    },
    enabled: !!reservationId,
  });
};
