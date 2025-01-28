import { useMutation, useQuery } from "@tanstack/react-query";
import { Category } from "../types/Category";
import { fetchData } from "./apiUtils";
import CryptoJS from "crypto-js";
import { User } from "src/types/UserTypes";
import { usePagination } from "src/hooks/usePagination";
import useCookies from "src/hooks/useCookies";

const SECRET_KEY = "your_secret_key";

const decryptData = (encryptedData: string): User | null => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedData, SECRET_KEY);
    const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
    return JSON.parse(decryptedData) as User;
  } catch (error) {
    console.error("Error decrypting user data:", error);
    return null;
  }
};

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

export const useGetCategoryById = (id: string | undefined) => {
  const { get } = useCookies();

  return useQuery<Category, Error>({
    queryKey: ["category", id],
    queryFn: async () => {
      if (!id) {
        throw new Error("No ID provided");
      }

      const encryptedUserData = get("user_data");
      if (!encryptedUserData) {
        throw new Error("No user data found in cookies.");
      }

      const userData = decryptData(encryptedUserData);
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
  const { get } = useCookies();

  return useMutation({
    mutationFn: async (formData: FormData) => {
      return await fetchData(
        "/api/Category",
        {
          method: "post",
          data: formData,
        },
        true
      );
    },
    onSuccess: () => {
      console.log("Category created successfully");
    },
  });
};

export const useUpdateCategory = () => {
  const { get } = useCookies();

  return useMutation({
    mutationFn: async (updatedCategory: { id: string; formData: FormData }) => {
      const encryptedUserData = get("user_data");

      if (!encryptedUserData) {
        throw new Error("No user data found in cookies.");
      }

      const userData = decryptData(encryptedUserData);

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
  const { get } = useCookies();

  return useMutation({
    mutationFn: async (updatedCategory: { id: string }) => {
      const encryptedUserData = get("user_data");

      if (!encryptedUserData) {
        throw new Error("No user data found in cookies.");
      }

      const userData = decryptData(encryptedUserData);

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
