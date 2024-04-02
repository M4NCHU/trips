import { FC } from "react";
import { Button } from "src/components/ui/button";

interface AdminNavIconProps {
  icon: React.ReactNode;
}

const AdminNavIcon: FC<AdminNavIconProps> = ({ icon }) => {
  return (
    <Button className="p-4 bg-secondary hover:bg-secondary/80 text-primary rounded-full">
      {icon}
    </Button>
  );
};

export default AdminNavIcon;
