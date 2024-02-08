// api/trips.timport { useQuery } from "@tanstack/react-query";
import { useMutation, useQuery } from "@tanstack/react-query";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";
import { LoginResponse, RegisterUser } from "src/types/UserTypes";
import { useContext } from "react";
import { useAuth } from "src/context/UserContext";
import toast from "react-hot-toast";

// Creating user
export const UseCreateUser = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      const response = await fetchData<RegisterUser>(
        "/api/Authentication/register",
        {
          method: "post",
          data: formData,
        }
      );
    },
  });
  return mutation;
};
export const UseLoginUser = () => {
  const mutation = useMutation<LoginResponse, Error, FormData>({
    mutationFn: async (formData: FormData) => {
      // Ensure fetchData returns a Promise<LoginResponse>
      return await fetchData<LoginResponse>("/api/Authentication/login", {
        method: "post",
        data: formData,
      });
    },
  });
  return mutation;
};

export const UseLogoutUser = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      try {
        await fetchData<boolean>("/api/Authentication/logout", {
          method: "post",
          data: formData,
        });
      } catch (error) {
        console.error("Error during logout:", error);
      }
    },
  });
  return mutation;
};

export const authorizedFetch = async <T>(
  url: string,
  options: RequestInit
): Promise<T> => {
  const token = localStorage.getItem("token");

  const headers = new Headers(options.headers);
  if (token) {
    headers.append("Authorization", `${token}`);
  }

  const response = await fetch(url, { ...options, headers });
  if (!response.ok) {
    throw new Error("Network response was not ok");
  }

  return response.json() as Promise<T>;
};
