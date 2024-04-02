import { FC } from "react";
import { Link } from "react-router-dom";

interface AdminSidebarHeaderProps {}

const AdminSidebarHeader: FC<AdminSidebarHeaderProps> = ({}) => {
  return (
    <div className="flex items-center mb-10 flex-row gap-2">
      <h1 className="text-xl font-semibold">Admin Panel</h1>
      <span>/</span>
      <Link to={"/"}>Home</Link>
    </div>
  );
};

export default AdminSidebarHeader;
