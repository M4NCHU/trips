import { FC } from "react";

interface AdminNavSearchProps {}

const AdminNavSearch: FC<AdminNavSearchProps> = ({}) => {
  return (
    <input
      className="bg-secondary rounded-full hidden md:block px-4 py-2"
      placeholder="Search for ... "
    />
  );
};

export default AdminNavSearch;
