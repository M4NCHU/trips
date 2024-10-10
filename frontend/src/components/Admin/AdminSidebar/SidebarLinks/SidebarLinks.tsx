import { FC } from "react";
import AdminSidebarHeader from "../SidebarLinks/AdminSidebarHeader";
import AdminSidebarSection from "../SidebarLinks/AdminSidebarSection";
import AdminSidebarSectionList from "../SidebarLinks/AdminSidebarSectionList";
import AdminSidebarSectionListItem from "../SidebarLinks/AdminSidebarSectionListItem";
import AdminSidebarSections from "../SidebarLinks/AdminSidebarSections";
import AdminSidebarFooter from "../SidebarLinks/AdminSidebarFooter";

interface SidebarLinksProps {}

const SidebarLinks: FC<SidebarLinksProps> = ({}) => {
  return (
    <>
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
    </>
  );
};

export default SidebarLinks;
