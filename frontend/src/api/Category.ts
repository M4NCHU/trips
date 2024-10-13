import { useMutation, useQuery } from "@tanstack/react-query";
import { Category } from "../types/Category";
import { fetchData } from "./apiUtils";
import {
  SECRET_KEY,
  USER_STORAGE_KEY,
  decryptData,
} from "src/context/UserContext";
import CryptoJS from "crypto-js";
import { User } from "src/types/UserTypes";
import { usePagination } from "src/hooks/usePagination";
import { PagedResult } from "src/types/PagedResult";

export const UseCategoryList = (filterParams: any = {}, pageSize = 2) => {
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
  } = usePagination<Category>("/api/Category", {
    pageSize: pageSize,
    queryParameters: filterParams,
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

// Zapytanie o kategorię po ID
export const useGetCategoryById = (id: string | undefined) => {
  return useQuery<Category, Error>({
    queryKey: ["category", id],
    queryFn: async () => {
      if (!id) {
        throw new Error("No ID provided");
      }

      const encryptedUserData = localStorage.getItem(USER_STORAGE_KEY);
      if (!encryptedUserData) {
        throw new Error("No user data found in local storage.");
      }

      const userData = decryptData(encryptedUserData, SECRET_KEY);
      if (!userData || !userData.token) {
        throw new Error("No JWT token found in user data.");
      }

      return fetchData<Category>(`/api/Category/${id}`, {
        method: "get",
        headers: {
          Authorization: `Bearer ${userData.token}`,
        },
      });
    },
    enabled: !!id,
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

export const useUpdateCategory = () => {
  return useMutation({
    mutationFn: async (updatedCategory: { id: string; formData: FormData }) => {
      const encryptedUserData = localStorage.getItem(USER_STORAGE_KEY);

      if (!encryptedUserData) {
        throw new Error("No user data found in local storage.");
      }

      const userData = decryptData(encryptedUserData, SECRET_KEY);

      if (!userData?.token) {
        throw new Error("No JWT token found in user data.");
      }

      return fetchData(`/api/Category/${updatedCategory.id}`, {
        method: "put",
        data: updatedCategory.formData,
        headers: {
          Authorization: `Bearer ${userData.token}`,
        },
      });
    },
  });
};

export const useDeleteCategory = () => {
  return useMutation({
    mutationFn: async (updatedCategory: { id: string }) => {
      const encryptedUserData = localStorage.getItem(USER_STORAGE_KEY);

      if (!encryptedUserData) {
        throw new Error("No user data found in local storage.");
      }

      const userData = decryptData(encryptedUserData, SECRET_KEY);

      if (!userData?.token) {
        throw new Error("No JWT token found in user data.");
      }

      return fetchData(`/api/Category/${updatedCategory.id}`, {
        method: "delete",
        headers: {
          Authorization: `Bearer ${userData.token}`,
        },
      });
    },
  });
};
