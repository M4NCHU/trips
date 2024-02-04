// api/trips.timport { useQuery } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import { Trip } from "../types/TripTypes";
import { fetchData } from "./apiUtils";
import { LoginResponse, RegisterUser } from "src/types/UserTypes";
import { useContext } from "react";
import { useAuth } from "src/context/UserContext";

// Creating user
export const UseCreateUser = async (formData: FormData) => {
  try {
    const response = await fetchData<RegisterUser>(
      "/api/Authentication/register",
      {
        method: "post",
        data: formData,
      }
    );

    return response;
  } catch (error) {
    console.error("Error creating new user:", error);
    throw new Error("Failed to create new user . Please try again.");
  }
};

export const UseLoginUser = async (formData: FormData) => {
  try {
    const response = await fetchData<LoginResponse>(
      "/api/Authentication/login",
      {
        method: "post",
        data: formData,
      }
    );

    return response;
  } catch (error) {
    console.error("Error creating new user:", error);
    throw new Error("Failed to create new user . Please try again.");
  }
};

export const logoutUser = async () => {
  const userDataJson = localStorage.getItem("user_data");

  if (!userDataJson) {
    console.error("No user data found during logout.");
    return;
  }

  const userData = JSON.parse(userDataJson);
  const token = userData.jwt;
  const userId = userData.id;

  if (!token) {
    console.error("No JWT token found in user data during logout.");
    return;
  }

  if (!userId) {
    console.error("No userId found in user data during logout.");
    return;
  }

  const formData = new FormData();

  formData.append("refreshToken", token);
  formData.append("userId", userId);

  try {
    const response = await fetchData<boolean>("/api/Authentication/logout", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error during logout:", error);
  }
};

export const authorizedFetch = async <T>(
  url: string,
  options: RequestInit
): Promise<T> => {
  const token = localStorage.getItem("token");

  const headers = new Headers(options.headers);
  if (token) {
    headers.append("Authorization", `Bearer ${token}`);
  }

  const response = await fetch(url, { ...options, headers });
  if (!response.ok) {
    throw new Error("Network response was not ok");
  }

  return response.json() as Promise<T>;
};
