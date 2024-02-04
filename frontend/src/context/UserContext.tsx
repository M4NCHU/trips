import React, { createContext, useState, useContext, useEffect } from "react";
import { logoutUser } from "src/api/AuthenticationAPI";
import { User } from "src/types/UserTypes";

interface UserContextType {
  user: User | null;
  setUser: (user: User | null) => void;
  logout: () => void;
}

const UserContext = createContext<UserContextType | null>(null);

const USER_STORAGE_KEY = "user_data";

const storeUser = (user: User) => {
  localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
};

const getUserFromStorage = (): User | null => {
  const userJSON = localStorage.getItem(USER_STORAGE_KEY);
  return userJSON ? JSON.parse(userJSON) : null;
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

  const logout = async () => {
    if (user?.token) {
      try {
        const responseLogout = await logoutUser();
        if (responseLogout) {
          localStorage.removeItem(USER_STORAGE_KEY);
          setUser(null);
        }
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

  console.log(user);
  return (
    <UserContext.Provider value={{ user, setUser, logout }}>
      {children}
    </UserContext.Provider>
  );
};
