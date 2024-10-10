import React, { FC } from "react";
import { Link } from "react-router-dom";

interface AdminLinkInterface {
  link: string;
  title: string;
}

const AdminLink: FC<AdminLinkInterface> = ({ link, title }) => {
  return (
    <li className="admin-nav-item">
      <Link
        to={link}
        className="text-lg font-normal px-4 py-2 hover:bg-background rounded-lg"
      >
        {title}
      </Link>
    </li>
  );
};

export default AdminLink;
