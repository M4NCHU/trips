// authUtils.ts
import { User } from "src/types/UserTypes";
import { fetchData } from "../api/apiUtils";
import { useAuth } from "src/context/UserContext";

export const isAuthenticated = async (): Promise<boolean> => {
  try {
    const user = await fetchCurrentUser();
    return !!user;
  } catch (error) {
    console.error("Błąd podczas sprawdzania autoryzacji:", error);
    return false;
  }
};

export const fetchCurrentUser = async (): Promise<User | null> => {
  try {
    const data = await fetchData<{ user: User }>("/api/Authentication/user", {
      method: "get",
    });
    return data.user;
  } catch (error) {
    console.error("Błąd podczas pobierania danych użytkownika:", error);
    return null;
  }
};

export const logoutUser = async (): Promise<void> => {
  try {
    await fetchData<void>("/api/Authentication/logout", {
      method: "post",
    });
  } catch (error) {
    console.error("Błąd podczas wylogowywania:", error);
  }
};
