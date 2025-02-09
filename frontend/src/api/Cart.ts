import { useQuery, useMutation } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { fetchData } from "./apiUtils";
import { CartItem } from "../types/Cart/CartItem";

export const useCart = () => {
  return useQuery<CartItem[], Error>({
    queryKey: ["cart"],
    queryFn: async () => {
      const response = await fetchData<{ result: CartItem[] }>(
        "/api/Cart/cart"
      );
      return response.result;
    },
  });
};

export const useAddToCart = () => {
  const mutation = useMutation({
    mutationFn: async (item: Omit<CartItem, "id">) => {
      try {
        const response = await fetchData<CartItem>("/api/Cart/add", {
          method: "post",
          data: item,
        });
        return response;
      } catch (error: any) {
        toast.error("Failed to add item to cart. Please try again.");
        throw new Error(error.message || "An error occurred.");
      }
    },
  });

  return mutation;
};

export const useRemoveFromCart = () => {
  const mutation = useMutation({
    mutationFn: async (itemId: string) => {
      try {
        const response = await fetchData(`/api/cart/remove`, {
          method: "post",
          data: { itemId },
        });
        toast.success("Item removed from cart successfully!");
        return response;
      } catch (error: any) {
        toast.error("Failed to remove item from cart. Please try again.");
        throw new Error(error.message || "An error occurred.");
      }
    },
  });

  return mutation;
};

export const useDecreaseQuantity = () => {
  const mutation = useMutation({
    mutationFn: async (item: Omit<CartItem, "id">) => {
      try {
        const response = await fetchData<CartItem>("/api/Cart/decrease", {
          method: "post",
          data: item,
        });
        toast.success("Quantity decreased successfully!");
        return response;
      } catch (error: any) {
        toast.error("Failed to decrease quantity. Please try again.");
        throw new Error(error.message || "An error occurred.");
      }
    },
  });

  return mutation;
};
