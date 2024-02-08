// api/Categorys.ts
import { useMutation, useQuery } from "@tanstack/react-query";
import { Category } from "../types/Category";
import { fetchData } from "./apiUtils";
import { useRoleChecker } from "src/hooks/useRoleChecker";

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
      const userDataJson = localStorage.getItem("user_data");
      const userData = userDataJson ? JSON.parse(userDataJson) : null;
      const token = userData?.jwt;

      return await fetchData<FormData>("/api/Category", {
        method: "post",
        data: formData,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
    },

    onSuccess: () => {},
  });

  return mutation;
};
