import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "src/context/UserContext";

const ProtectedRoute = ({ children }: any) => {
  const { user } = useAuth(); // Use your auth context or another method to check auth status

  if (!user || user === null) {
    return <Navigate to="/login" />;
  }

  return children;
};

export default ProtectedRoute;
