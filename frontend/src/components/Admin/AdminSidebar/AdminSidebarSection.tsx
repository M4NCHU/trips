import { Children, FC } from "react";
import AdminSidebarSectionTitle from "./AdminSidebarSectionTitle";

interface AdminSidebarSectionProps {
  title: string;
  children: React.ReactNode;
}

const AdminSidebarSection: FC<AdminSidebarSectionProps> = ({
  title,
  children,
}) => {
  return (
    <div className="">
      <AdminSidebarSectionTitle title={title} />
      {children}
    </div>
  );
};

export default AdminSidebarSection;
