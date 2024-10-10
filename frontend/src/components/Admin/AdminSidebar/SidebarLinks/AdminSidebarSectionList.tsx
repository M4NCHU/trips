import { FC } from "react";

interface AdminSidebarSectionListProps {
  children: React.ReactNode;
}

const AdminSidebarSectionList: FC<AdminSidebarSectionListProps> = ({
  children,
}) => {
  return <ul className="flex flex-col">{children}</ul>;
};

export default AdminSidebarSectionList;
