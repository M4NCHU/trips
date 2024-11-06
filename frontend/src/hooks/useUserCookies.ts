import useCookies from "../hooks/useCookies";
import CryptoJS from "crypto-js";
import { User } from "src/types/UserTypes";
import { fetchData } from "src/api/apiUtils";

const SECRET_KEY = "your_secret_key";

export const useUserCookies = () => {
  const { get, set, remove } = useCookies();

  const storeUserInCookies = (user: User): void => {
    const encryptedData = CryptoJS.AES.encrypt(
      JSON.stringify(user),
      SECRET_KEY
    ).toString();
    set("user_data", encryptedData, 7);
  };

  const getUserFromCookies = (): User | null => {
    const encryptedData = get("user_data");
    if (!encryptedData) return null;

    try {
      const bytes = CryptoJS.AES.decrypt(encryptedData, SECRET_KEY);
      const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
      return JSON.parse(decryptedData) as User;
    } catch (error) {
      console.error("Error decrypting user data:", error);
      return null;
    }
  };

  const removeUserFromCookies = (): void => {
    remove("user_data");
  };

  const decodeToken = (token: string): { exp: number } | null => {
    try {
      const payload = token.split(".")[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload) as { exp: number };
    } catch (error) {
      console.error("Error decoding token:", error);
      return null;
    }
  };

  const checkTokenExpiration = async () => {
    const user = getUserFromCookies();
    if (!user || !user.token) return;

    const decodedToken = decodeToken(user.token);
    if (!decodedToken) return;

    const currentTime = Math.floor(Date.now() / 1000);
    const tokenExpirationTime = decodedToken.exp;
    const timeUntilExpiration = tokenExpirationTime - currentTime;

    if (timeUntilExpiration < 300) {
      console.log("Token is about to expire, refreshing token...");
      await refreshAuthToken();
    }
  };

  const refreshAuthToken = async () => {
    const user = getUserFromCookies();

    if (!user || !user.token || !user.id) {
      console.error("No user, token, or user ID found");
      return;
    }

    const formData = new FormData();
    formData.append("refreshToken", user.token);
    formData.append("userId", user.id);

    try {
      const response = await fetchData<User>("/api/Authentication/new-token", {
        method: "post",
        data: formData,
      });

      if (response && response.token) {
        // Zaktualizuj dane użytkownika
        const updatedUser = {
          ...user,
          token: response.token,
        };
        storeUserInCookies(updatedUser);
        console.log("Token refreshed successfully");
      } else {
        console.error("Failed to refresh token");
      }
    } catch (error) {
      console.error("Error refreshing token:", error);
    }
  };

  return {
    storeUserInCookies,
    getUserFromCookies,
    removeUserFromCookies,
    checkTokenExpiration,
    refreshAuthToken,
  };
};
