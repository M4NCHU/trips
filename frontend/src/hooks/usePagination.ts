import { useState, useEffect } from "react";
import { useQuery, UseQueryResult } from "@tanstack/react-query";
import { useLocation, useNavigate } from "react-router-dom";
import { fetchData } from "../api/apiUtils";
import { PagedResult } from "src/types/PagedResult";

interface PaginationOptions {
  pageSize?: number;
  queryParameters?: Record<string, string | number>;
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

  const fetchPage = async (): Promise<PagedResult<T>> => {
    const data = await fetchData<PagedResult<T>>(
      url,
      { method: "get" },
      true,
      queryParameters,
      { page, pageSize }
    );

    return data;
  };

  const {
    isLoading,
    isError,
    error,
    data,
    isFetching,
  }: UseQueryResult<PagedResult<T>, Error> = useQuery({
    queryKey: [url, page, pageSize, JSON.stringify(queryParameters)],
    queryFn: fetchPage,
    staleTime: 5000,
    placeholderData: {
      items: [],
      totalItems: 0,
      pageSize: pageSize,
      currentPage: page,
    } as PagedResult<T>,
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
