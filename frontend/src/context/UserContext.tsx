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

const UserContext = createContext<UserContextType | null>(null);

const SECRET_KEY = "your_secret_key";
const USER_STORAGE_KEY = "user_data";

const storeUser = (user: User) => {
  const userJSON = JSON.stringify(user);
  const encryptedData = CryptoJS.AES.encrypt(userJSON, SECRET_KEY).toString();
  localStorage.setItem(USER_STORAGE_KEY, encryptedData);
};

const getUserFromStorage = (): User | null => {
  const encryptedData = localStorage.getItem(USER_STORAGE_KEY);
  if (!encryptedData) return null;

  try {
    const bytes = CryptoJS.AES.decrypt(encryptedData, SECRET_KEY);
    const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
    return JSON.parse(decryptedData);
  } catch (error) {
    console.error("Error decrypting user data:", error);
    return null;
  }
};

export const useAuth = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useUser must be used within a UserProvider");
  }
  return context;
};

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(getUserFromStorage());

  const { mutate: logoutUser } = UseLogoutUser();

  const logout = async () => {
    if (user?.token) {
      const userDataJson = localStorage.getItem("user_data");

      if (!userDataJson) {
        console.error("No user data found during logout.");
        return;
      }

      const userData = JSON.parse(userDataJson);
      const token = userData.jwt;
      const userId = userData.id;

      if (!token) {
        console.error("No JWT token found in user data during logout.");
        return;
      }

      if (!userId) {
        console.error("No userId found in user data during logout.");
        return;
      }

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
            console.error("Error submitting form:", error);
            toast.error("Failed to create category.");
          },
        });
      } catch (error: any) {
        console.error("Error submitting form:", error);
      }
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
