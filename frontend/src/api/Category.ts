// api/Categorys.ts
import { useMutation, useQuery } from "@tanstack/react-query";
import { Category } from "../types/Category";
import { fetchData } from "./apiUtils";
import { useRoleChecker } from "src/hooks/useRoleChecker";
import {
  SECRET_KEY,
  USER_STORAGE_KEY,
  decryptData,
} from "src/context/UserContext";
import CryptoJS from "crypto-js";
import { User } from "src/types/UserTypes";

// Get Categories with pagination
export const UseCategoryList = () => {
  return useQuery<Category[], Error>({
    queryKey: ["categories"],
    queryFn: async () => {
      return fetchData<Category[]>("/api/Category");
    },
  });
};

export const useCreateCategory = () => {
  const mutation = useMutation({
    mutationFn: async (formData: FormData) => {
      const encryptedUserData = localStorage.getItem(USER_STORAGE_KEY);
      if (!encryptedUserData) {
        throw new Error("No user data found in local storage.");
      }
      const userData = decryptData(encryptedUserData, SECRET_KEY);

      if (!userData || !userData.token) {
        throw new Error("No JWT token found in user data.");
      }

      try {
        return await fetchData("/api/Category", {
          method: "post",
          data: formData,
          headers: {
            Authorization: `Bearer ${userData.token}`,
          },
        });
      } catch (error) {
        console.error("Error fetching data:", error);
        throw error;
      }
    },
    onSuccess: () => {},
  });

  return mutation;
};
