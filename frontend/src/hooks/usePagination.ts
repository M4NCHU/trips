import { useQuery, UseQueryResult } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { fetchDataPaginated } from "../api/apiUtils";
import { PagedResult } from "src/types/PagedResult";

// Typowanie dla opcji paginacji
interface PaginationOptions {
  pageSize: number;
  queryParameters?: Record<string, any>;
}

export const usePagination = <T>(url: string, options?: PaginationOptions) => {
  const { pageSize = 10, queryParameters = {} } = options || {};
  const [page, setPage] = useState<number>(1);
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const pageParam = parseInt(searchParams.get("page") || "1", 10);
    setPage(pageParam);
  }, [location]);

  const fetchPage = async (
    page: number,
    pageSize: number
  ): Promise<PagedResult<T>> => {
    const data = await fetchDataPaginated<T>(
      url,
      page,
      pageSize,
      queryParameters
    );

    return {
      items: data.items,
      totalItems: data.totalItems,
      pageSize: data.pageSize,
      currentPage: data.currentPage,
    };
  };

  const {
    isLoading,
    isError,
    error,
    data,
    isFetching,
  }: UseQueryResult<PagedResult<T>, Error> = useQuery({
    queryKey: [url, page, pageSize, queryParameters],
    queryFn: () => fetchPage(page, pageSize),
    staleTime: 5000,
    placeholderData: {
      items: [],
      totalItems: 0,
      pageSize: pageSize,
      currentPage: page,
    },
  });

  return {
    isLoading,
    isError,
    error,
    data: data?.items || [],
    totalItems: data?.totalItems || 0,
    pageSize: data?.pageSize || pageSize,
    currentPage: data?.currentPage || page,
    isFetching,
    page,
    setPage,
  };
};
