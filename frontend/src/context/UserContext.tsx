import React, { createContext, useState, useContext, useEffect } from "react";
import toast from "react-hot-toast";
import { User } from "src/types/UserTypes";
import { fetchCurrentUser, logoutUser } from "src/utils/authUtils";
import useCookies from "../hooks/useCookies";

interface UserContextType {
  user: User | null;
  setUser: (user: User | null) => void;
  logout: () => void;
  isAuthenticated: () => boolean;
  isLoading: boolean;
}

const UserContext = createContext<UserContextType | null>(null);

export const useAuth = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useAuth must be used within a UserProvider");
  }
  return context;
};

export const UserProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const { get, set } = useCookies();
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const checkAuthentication = async () => {
      const isAuthenticatedCookie = get("isAuthenticated");

      if (isAuthenticatedCookie === "true") {
        try {
          const fetchedUser = await fetchCurrentUser();
          setUser(fetchedUser);
        } catch (error) {
          console.error("Błąd podczas pobierania użytkownika:", error);
          setUser(null);
        }
      } else {
        setUser(null);
      }

      setIsLoading(false);
    };

    checkAuthentication();
  }, []);

  const logout = async () => {
    try {
      await logoutUser();
      setUser(null);
      set("isAuthenticated", "false", 7);
      toast.success("Pomyślnie wylogowano!");
    } catch (error) {
      console.error("Błąd podczas wylogowywania:", error);
      toast.error("Wylogowanie nie powiodło się.");
    }
  };

  const isAuthenticated = (): boolean => {
    return user !== null && get("isAuthenticated") === "true";
  };

  return (
    <UserContext.Provider
      value={{ user, setUser, logout, isAuthenticated, isLoading }}
    >
      {children}
    </UserContext.Provider>
  );
};
