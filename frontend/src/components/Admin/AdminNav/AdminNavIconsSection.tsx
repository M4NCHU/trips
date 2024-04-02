import { FC } from "react";

interface AdminNavIconsSectionProps {
  children: React.ReactNode;
}

const AdminNavIconsSection: FC<AdminNavIconsSectionProps> = ({ children }) => {
  return (
    <div className="hidden md:flex flex-row gap-2 flex-wrap">{children}</div>
  );
};

export default AdminNavIconsSection;
