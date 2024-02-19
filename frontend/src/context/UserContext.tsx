import React, { createContext, useState, useContext, useEffect } from "react";
import toast from "react-hot-toast";
import { UseLogoutUser } from "src/api/AuthenticationAPI";
import { User } from "src/types/UserTypes";
import CryptoJS from "crypto-js";

interface UserContextType {
  user: User | null;
  setUser: (user: User | null) => void;
  logout: () => void;
}

interface EncryptedData {
  token: string;
  id: string;
}

const UserContext = createContext<UserContextType | null>(null);

export const SECRET_KEY = "your_secret_key";
export const USER_STORAGE_KEY = "user_data";

export const useAuth = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useUser must be used within a UserProvider");
  }
  return context;
};

export const encryptData = (
  data: User | EncryptedData,
  secretKey: string
): string => {
  return CryptoJS.AES.encrypt(JSON.stringify(data), secretKey).toString();
};

export const decryptData = (
  encryptedData: string,
  secretKey: string
): User | null => {
  try {
    const bytes = CryptoJS.AES.decrypt(encryptedData, secretKey);
    const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
    return JSON.parse(decryptedData) as User;
  } catch (error) {
    console.error("Error decrypting data:", error);
    return null;
  }
};

export const storeUser = (user: User): void => {
  const encryptedData = encryptData(user, SECRET_KEY);
  localStorage.setItem(USER_STORAGE_KEY, encryptedData);
};

export const getUserFromStorage = (): User | null => {
  const encryptedData = localStorage.getItem(USER_STORAGE_KEY);
  if (!encryptedData) return null;
  return decryptData(encryptedData, SECRET_KEY);
};

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(getUserFromStorage());

  const { mutate: logoutUser } = UseLogoutUser();

  const logout = async () => {
    // First, get the encrypted user data from local storage.
    const encryptedUserData = localStorage.getItem(USER_STORAGE_KEY);

    if (!encryptedUserData) {
      console.error("No user data found during logout.");
      return;
    }

    // Decrypt the user data.
    let userData;
    try {
      const bytes = CryptoJS.AES.decrypt(encryptedUserData, SECRET_KEY);
      const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
      userData = JSON.parse(decryptedData);
    } catch (error) {
      console.error("Error decrypting user data:", error);
      return;
    }

    // Extract necessary data for logout.
    const token = userData?.token; // Assuming the token is stored directly under the user object.
    const userId = userData?.id;

    if (!token) {
      console.error("No JWT token found in user data during logout.");
      return;
    }

    if (!userId) {
      console.error("No userId found in user data during logout.");
      return;
    }

    // Prepare the form data for the logout request.
    const formData = new FormData();
    formData.append("refreshToken", token);
    formData.append("userId", userId);

    try {
      logoutUser(formData, {
        onSuccess: () => {
          toast.success("Logged out successfully!");
          localStorage.removeItem(USER_STORAGE_KEY);
          setUser(null);
        },
        onError: (error: any) => {
          console.error("Error during logout:", error);
          toast.error("Logout failed.");
        },
      });
    } catch (error: any) {
      console.error("Error during logout:", error);
    }
  };

  useEffect(() => {
    if (user) {
      storeUser(user);
    } else {
      localStorage.removeItem(USER_STORAGE_KEY);
    }
  }, [user]);

  return (
    <UserContext.Provider value={{ user, setUser, logout }}>
      {children}
    </UserContext.Provider>
  );
};
