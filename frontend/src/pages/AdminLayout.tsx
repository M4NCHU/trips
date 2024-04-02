import { FC } from "react";
import { Navigate, Outlet, useNavigate } from "react-router-dom";
import Header from "../components/Header/Header";
import AdminSidebar from "src/components/Admin/AdminSidebar/AdminSidebar";
import AdminNav from "src/components/Admin/AdminNav/AdminNav";
import { useAuth } from "src/context/UserContext";
import { useRoleChecker } from "src/hooks/useRoleChecker";

interface LayoutProps {
  children: React.ReactNode;
}

const AdminLayout: FC<LayoutProps> = ({ children }) => {
  const { user } = useAuth();
  const { hasRole } = useRoleChecker();
  const navigate = useNavigate();
  const roles = ["admin"];

  if (!user) {
    return <Navigate to="/login" />;
  }

  if (roles && !roles.some((role) => hasRole(role))) {
    return <Navigate to="/" />;
  }

  return (
    <div className="flex flex-row h-screen">
      <AdminSidebar />
      <div className="flex flex-col grow p-4 gap-4 h-screen">
        <AdminNav />
        <div className=" rounded-lg grow flex flex-col">{children}</div>
      </div>
      <Outlet />
    </div>
  );
};

export default AdminLayout;
