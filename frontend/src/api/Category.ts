// api/Categorys.ts
import { useQuery } from "@tanstack/react-query";
import { usePagination } from "../hooks/usePagination";
import { Category } from "../types/Category";
import { fetchData } from "./apiUtils";

// Get Categories with pagination
export const GetCategoryList = () => {
  return useQuery<Category[], Error>({
    queryKey: ["categories"],
    queryFn: async () => {
      return fetchData<Category[]>("/api/category/GetAllCategories");
    },
  });
};

export const createCategory = async (formData: FormData) => {
  try {
    const response = await fetchData<Category>("/api/category/CreateCategory", {
      method: "post",
      data: formData,
    });

    return response;
  } catch (error) {
    console.error("Error creating Category:", error);
    throw new Error("Failed to create Category. Please try again.");
  }
};
