import { FC } from "react";
import AdminSidebarHeader from "./AdminSidebarHeader";
import AdminSidebarSection from "./AdminSidebarSection";
import AdminSidebarSectionList from "./AdminSidebarSectionList";
import AdminSidebarSectionListItem from "./AdminSidebarSectionListItem";
import AdminSidebarSections from "./AdminSidebarSections";
import AdminSidebarFooter from "./AdminSidebarFooter";

interface AdminSidebarProps {}

const AdminSidebar: FC<AdminSidebarProps> = ({}) => {
  return (
    <div className="bg-secondary w-1/4 p-4 h-screen hidden md:flex flex-col">
      <AdminSidebarHeader />
      <AdminSidebarSections>
        <AdminSidebarSection title="basic">
          <AdminSidebarSectionList>
            <AdminSidebarSectionListItem link="/admin" title="Dashboard" />
            <AdminSidebarSectionListItem
              link="/admin/destinations/create"
              title="Destinations"
            />
            <AdminSidebarSectionListItem link="/admin" title="Categories" />
            <AdminSidebarSectionListItem link="/admin" title="Visit Places" />
          </AdminSidebarSectionList>
        </AdminSidebarSection>
      </AdminSidebarSections>
      <AdminSidebarFooter />
    </div>
  );
};

export default AdminSidebar;
