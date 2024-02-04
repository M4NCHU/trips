import { useAuth } from "src/context/UserContext";

export const useRoleChecker = () => {
  const { user } = useAuth();

  const hasRole = (role: string) => {
    // Zakładając, że role są przechowywane jako tablica w user.roles
    return user?.roles?.includes(role);
  };

  return { hasRole };
};
