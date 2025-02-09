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
  const { user, isAuthenticated, isLoading } = useAuth();
  const { hasRole } = useRoleChecker();

  console.log("User:", user);
  console.log("isAuthenticated:", isAuthenticated());
  console.log("isLoading:", isLoading);

  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (!isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }

  if (roles && !roles.some((role) => hasRole(role))) {
    return <Navigate to="/" replace />;
  }

  return children;
};

export default ProtectedRoute;
