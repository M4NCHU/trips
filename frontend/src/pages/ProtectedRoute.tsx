import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "src/context/UserContext";
import { useRoleChecker } from "src/hooks/useRoleChecker";

const ProtectedRoute = ({
  children,
  roles,
}: {
  children: any;
  roles?: string[];
}) => {
  const { user } = useAuth();
  const { hasRole } = useRoleChecker();

  if (!user) {
    return <Navigate to="/login" />;
  }

  // Jeśli trasa wymaga określonych ról, sprawdź, czy użytkownik je posiada
  if (roles && !roles.some((role) => hasRole(role))) {
    return <div>Access Denied</div>; // Możesz tu przekierować na inną stronę, jeśli chcesz
  }

  return children;
};

export default ProtectedRoute;
