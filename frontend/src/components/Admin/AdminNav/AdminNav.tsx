import { FC } from "react";
import AdminNavTitle from "./AdminNavTitle";
import AdminNavSearch from "./AdminNavSearch";
import AdminNavIconsSection from "./AdminNavIconsSection";
import AdminNavIcon from "./AdminNavIcon";
import { BsSun } from "react-icons/bs";
import UserIconWithDropdown from "src/components/User/UserIconWithDropdown";

interface AdminNavProps {}

const AdminNav: FC<AdminNavProps> = ({}) => {
  return (
    <div className="flex flex-row justify-between">
      <AdminNavTitle title="Dashboard" date="01-04-2024" />
      <div className="flex flex-row items-center gap-2">
        <AdminNavSearch />
        <AdminNavIconsSection>
          <AdminNavIcon icon={<BsSun />} />
          <AdminNavIcon icon={<BsSun />} />
          <AdminNavIcon icon={<BsSun />} />
          <UserIconWithDropdown />
        </AdminNavIconsSection>
      </div>
    </div>
  );
};

export default AdminNav;
