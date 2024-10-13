import axios, { AxiosResponse, AxiosError } from "axios";
import { PagedResult } from "src/types/PagedResult";

axios.defaults.baseURL = process.env.REACT_APP_API_BASE_URL;

interface FetchDataOptions {
  method?: "get" | "post" | "put" | "delete" | "patch";
  data?: any;
  headers?: { [key: string]: string };
}

export const fetchDataPaginated = async <T>(
  url: string,
  page: number,
  pageSize: number,
  queryParameters: Record<string, string | number> = {}
): Promise<PagedResult<T>> => {
  try {
    const queryParams = new URLSearchParams(
      queryParameters as Record<string, string>
    );
    queryParams.append("page", page.toString());
    queryParams.append("pageSize", pageSize.toString());

    // Assuming API returns an object containing `items`, `totalItems`, etc.
    const response: AxiosResponse<PagedResult<T>> = await axios.get(
      `${url}?${queryParams.toString()}`
    );

    return response.data; // Directly return the `PagedResult<T>` from the API.
  } catch (error) {
    throw handleAxiosError(error);
  }
};

export const fetchData = async <T>(
  url: string,
  options?: FetchDataOptions
): Promise<T> => {
  try {
    const { method = "get", data = null, headers = {} } = options || {};
    const response: AxiosResponse<T> = await axios({
      method,
      url,
      data,
      headers,
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
        axiosError.response.data?.message || "Wystąpił błąd zapytania.";
      const status = axiosError.response.status;
      throw new Error(`${message} (Status: ${status})`);
    } else if (axiosError.request) {
      throw new Error("Brak odpowiedzi od serwera.");
    }
  }

  throw new Error("Wystąpił nieznany błąd.");
}
