import axios, { AxiosResponse, AxiosError } from "axios";
import { PagedResult } from "src/types/PagedResult";
import { isAuthenticated } from "../utils/authUtils";
import { getCookie } from "src/utils/cookiesUtils";

axios.defaults.baseURL =
  process.env.REACT_APP_API_BASE_URL || "http://localhost:5000";

interface FetchDataOptions {
  method?: "get" | "post" | "put" | "delete" | "patch";
  data?: any;
  headers?: { [key: string]: string };
}

interface PaginationParams {
  page: number;
  pageSize: number;
}

export const fetchData = async <T>(
  url: string,
  options?: FetchDataOptions,
  includeAuth: boolean = false,
  queryParameters?: Record<string, string | number>,
  pagination?: PaginationParams
): Promise<T> => {
  try {
    const { method = "get", data = null, headers = {} } = options || {};

    const queryParams = new URLSearchParams();

    if (queryParameters) {
      Object.entries(queryParameters).forEach(([key, value]) => {
        queryParams.append(key, value.toString());
      });
    }

    if (pagination) {
      queryParams.append("page", pagination.page.toString());
      queryParams.append("pageSize", pagination.pageSize.toString());
    }

    const fullUrl = queryParams.toString() ? `${url}?${queryParams}` : url;

    const authHeader = includeAuth
      ? { Authorization: `Bearer ${getCookie("jwt")}` }
      : {};

    const response: AxiosResponse<T> = await axios({
      method,
      url: fullUrl,
      data,
      headers: { ...headers, ...authHeader },
    });
    return response.data;
  } catch (error) {
    throw handleAxiosError(error);
  }
};

export const fetchDataPaginated = async <T>(
  url: string,
  page: number,
  pageSize: number,
  queryParameters: Record<string, string | number> = {},
  includeAuth: boolean = false
): Promise<PagedResult<T>> => {
  try {
    if (includeAuth && !isAuthenticated()) {
      throw new Error("User is not authenticated. Authorization required.");
    }

    const queryParams = new URLSearchParams(
      queryParameters as Record<string, string>
    );
    queryParams.append("page", page.toString());
    queryParams.append("pageSize", pageSize.toString());

    const fullUrl = `${url}?${queryParams.toString()}`;

    const response: AxiosResponse<PagedResult<T>> = await axios.get(fullUrl, {
      withCredentials: includeAuth,
    });
    return response.data;
  } catch (error) {
    throw handleAxiosError(error);
  }
};

function handleAxiosError(error: unknown) {
  if (axios.isAxiosError(error)) {
    const axiosError = error as AxiosError<{ message?: string }>;

    if (axiosError.response) {
      const message =
        axiosError.response.data?.message ||
        "An error occurred during the request.";
      const status = axiosError.response.status;
      throw new Error(`${message} (Status: ${status})`);
    } else if (axiosError.request) {
      throw new Error("No response from the server.");
    }
  }

  throw new Error("An unknown error occurred.");
}
