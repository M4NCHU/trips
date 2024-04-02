import { Children, FC } from "react";

interface AdminSidebarSectionsProps {
  children: React.ReactNode;
}

const AdminSidebarSections: FC<AdminSidebarSectionsProps> = ({ children }) => {
  return <div className="grow overflow-y-auto">{children}</div>;
};

export default AdminSidebarSections;
