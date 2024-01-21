import axios, { AxiosResponse, AxiosError } from "axios";

axios.defaults.baseURL = process.env.REACT_APP_API_BASE_URL;

interface FetchDataOptions {
  method?: "get" | "post" | "put" | "delete";
  data?: any;
}

export const fetchDataPaginated = async <T>(
  url: string,
  page: number,
  pageSize: number
): Promise<{ data: T[]; hasMore: boolean }> => {
  try {
    const response: AxiosResponse<T[]> = await axios.get<T[]>(
      `${url}?page=${page}&pageSize=${pageSize}`
    );
    return {
      data: response.data,
      hasMore: response.data.length === pageSize,
    };
  } catch (error) {
    throw handleAxiosError(error);
  }
};

export const fetchData = async <T>(
  url: string,
  options?: FetchDataOptions
): Promise<T> => {
  try {
    const { method = "get", data = null } = options || {};
    const response: AxiosResponse<T> = await axios({ method, url, data });
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
