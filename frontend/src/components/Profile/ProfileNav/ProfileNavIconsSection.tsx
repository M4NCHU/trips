import { FC } from "react";

interface ProfileNavIconsSectionProps {
  children: React.ReactNode;
}

const ProfileNavIconsSection: FC<ProfileNavIconsSectionProps> = ({
  children,
}) => {
  return (
    <div className="hidden md:flex flex-row gap-2 flex-wrap">{children}</div>
  );
};

export default ProfileNavIconsSection;
