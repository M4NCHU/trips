import { FC } from "react";
import { Link } from "react-router-dom";

interface AdminSidebarSectionListItemProps {
  link: string;
  title: string;
}

const AdminSidebarSectionListItem: FC<AdminSidebarSectionListItemProps> = ({
  link,
  title,
}) => {
  return (
    <Link to={link} className="p-4 hover:bg-background rounded-full">
      {title}
    </Link>
  );
};

export default AdminSidebarSectionListItem;
