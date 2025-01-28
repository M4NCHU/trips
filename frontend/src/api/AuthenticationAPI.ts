import { useMutation } from "@tanstack/react-query";
import { fetchData } from "./apiUtils";
import { LoginResponse, RegisterUser } from "src/types/UserTypes";
import toast from "react-hot-toast";

export const UseCreateUser = () => {
  const mutation = useMutation<RegisterUser, Error, FormData>({
    mutationFn: async (formData: FormData) => {
      return await fetchData<RegisterUser>("/api/Authentication/register", {
        method: "post",
        data: formData,
      });
    },
  });
  return mutation;
};

export const UseLoginUser = () => {
  const mutation = useMutation<LoginResponse, Error, FormData>({
    mutationFn: async (formData: FormData) => {
      return await fetchData<LoginResponse>("/api/Authentication/login", {
        method: "post",
        data: formData,
      });
    },
  });
  return mutation;
};

export const UseLogoutUser = () => {
  const mutation = useMutation<void, Error>({
    mutationFn: async () => {
      await fetchData<void>("/api/Authentication/logout", {
        method: "post",
      });
    },
  });
  return mutation;
};

export const UseRefreshToken = () => {
  const mutation = useMutation<void, Error>({
    mutationFn: async () => {
      await fetchData<void>("/api/Authentication/refresh-token", {
        method: "post",
      });
    },
    onError: (error) => {
      console.error("Error refreshing token:", error);
      toast.error("Session expired. Please log in again.");
    },
  });
  return mutation;
};
