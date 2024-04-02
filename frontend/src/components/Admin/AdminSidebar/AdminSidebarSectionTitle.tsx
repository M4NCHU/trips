import { FC } from "react";

interface AdminSidebarSectionTitleProps {
  title: string;
}

const AdminSidebarSectionTitle: FC<AdminSidebarSectionTitleProps> = ({
  title,
}) => {
  return (
    <div className="py-2">
      <h1 className="text-gray-400">{title}</h1>
    </div>
  );
};

export default AdminSidebarSectionTitle;
