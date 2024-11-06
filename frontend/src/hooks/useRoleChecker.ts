import { useAuth } from "src/context/UserContext";

export const useRoleChecker = () => {
  const { user } = useAuth();

  const hasRole = (role: string) => {
    return user?.roles?.includes(role);
  };

  return { hasRole };
};
