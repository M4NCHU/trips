import axios, { AxiosResponse, AxiosError } from "axios";

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

interface FetchDataOptions {
  method?: string;
  data?: any;
}

// Zmieniona funkcja fetchDataPaginated, która korzysta z paginacji
export const fetchDataPaginated = async <T>(
  url: string,
  page: number,
  pageSize: number
) => {
  const response: AxiosResponse<T[]> = await axios.get<T[]>(
    `${API_BASE_URL}${url}?page=${page}&pageSize=${pageSize}`
  );

  return {
    data: response.data,
    hasMore: response.data.length === pageSize,
  };
};

export const fetchData = async <T>(url: string, options?: FetchDataOptions) => {
  const { method = "get", data = null } = options || {};

  try {
    const response: AxiosResponse<T> = await axios({
      method,
      url: `${API_BASE_URL}${url}`,
      data,
    });

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const axiosError: AxiosError = error;

      if (axiosError.response) {
        // Błąd z odpowiedzią od serwera (np. status HTTP nie w zakresie 2xx)
        throw new Error(
          (axiosError.response.data as { message?: string })?.message ||
            "Wystąpił błąd zapytania."
        );
      } else if (axiosError.request) {
        // Błąd dotyczący samego zapytania (np. brak odpowiedzi)
        throw new Error("Brak odpowiedzi od serwera.");
      }
    }

    // Inny błąd
    throw new Error("Wystąpił nieznany błąd.");
  }
};
