import { FC } from "react";
import SidebarIcons from "./SidebarIcons/SidebarIcons";
import SidebarLinks from "./SidebarLinks/SidebarLinks";

interface AdminSidebarProps {}

const AdminSidebar: FC<AdminSidebarProps> = ({}) => {
  return (
    <div className="bg-background w-1/4 h-screen hidden md:flex flex-row">
      <SidebarIcons />
      <SidebarLinks />
    </div>
  );
};

export default AdminSidebar;
