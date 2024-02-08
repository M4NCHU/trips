import { Navigate, useNavigate } from "react-router-dom";
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
  const navigate = useNavigate();

  if (!user) {
    return <Navigate to="/login" />;
  }

  if (roles && !roles.some((role) => hasRole(role))) {
    return <Navigate to="/" />;
  }

  return children;
};

export default ProtectedRoute;
