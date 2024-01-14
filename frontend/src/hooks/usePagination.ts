// hooks/usePagination.ts
import {
  useQuery,
  useQueryClient,
  keepPreviousData,
} from "@tanstack/react-query";
import { fetchDataPaginated } from "../api/apiUtils";
import { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

interface PaginationOptions {
  pageSize: number;
}

interface PaginatedData<T> {
  data: T[];
  hasMore: boolean;
}

export const usePagination = <T>(url: string, options?: PaginationOptions) => {
  const queryClient = useQueryClient();
  const { pageSize = 2 } = options || {};
  const [page, setPage] = useState(1);
  const location = useLocation();
  const history = useNavigate();

  // Aktualizacja numeru strony na podstawie adresu URL
  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const pageParam = parseInt(searchParams.get("page") || "1", 10);
    setPage(pageParam);
  }, [location]);

  const fetchPage = async (
    page: number,
    pageSize: number
  ): Promise<PaginatedData<T>> => {
    const data = await fetchDataPaginated<T>(url, page, pageSize);
    return { data: data.data, hasMore: data.hasMore };
  };

  const fetchNextPage = async () => {
    if (!isPlaceholderData && data?.hasMore) {
      setPage((old) => old + 1);
      history({ search: `?page=${page + 1}` });
    }
  };

  const fetchPreviousPage = async () => {
    if (page > 1) {
      setPage((old) => old - 1);
      history({ search: `?page=${page - 1}` });
    }
  };

  const { isPending, isError, error, data, isFetching, isPlaceholderData } =
    useQuery<PaginatedData<T>>({
      queryKey: [url, page, pageSize],
      queryFn: () => fetchPage(page, pageSize),
      placeholderData: keepPreviousData,
      staleTime: 5000,
    });

  return {
    isPending,
    isError,
    error,
    data: data?.data || [],
    isFetching,
    isPlaceholderData,
    fetchNextPage,
    fetchPreviousPage,
    page,
    setPage,
  };
};
