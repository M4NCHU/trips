import { FC } from "react";
import { Button } from "src/components/ui/button";

interface ProfileNavIconProps {
  icon: React.ReactNode;
}

const ProfileNavIcon: FC<ProfileNavIconProps> = ({ icon }) => {
  return (
    <Button className="p-4 bg-secondary hover:bg-secondary/80 text-primary rounded-full">
      {icon}
    </Button>
  );
};

export default ProfileNavIcon;
