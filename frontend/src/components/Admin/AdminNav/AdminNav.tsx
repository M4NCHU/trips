import { FC } from "react";
import { Link } from "react-router-dom";
import AdminNavTitle from "./AdminNavTitle";
import AdminLink from "./AdminLink";

interface AdminNavProps {}

const AdminNav: FC<AdminNavProps> = ({}) => {
  return (
    <div className="flex flex-row justify-between py-2 border-b-[1px] border-background">
      <AdminNavTitle title="Admin Panel" date="01-04-2024" />
      <div className="flex flex-row items-center gap-2 py-2">
        <ul className="admin-link-ul flex flex-row">
          <AdminLink link={"/admin/destinations"} title={"Destinations"} />
          <AdminLink link={"/admin/categories"} title={"Categories"} />
        </ul>
      </div>
    </div>
  );
};

export default AdminNav;
