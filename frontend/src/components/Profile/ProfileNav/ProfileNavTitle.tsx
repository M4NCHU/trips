import { FC } from "react";

interface ProfileNavTitleProps {
  title: string;
  date?: string;
}

const ProfileNavTitle: FC<ProfileNavTitleProps> = ({ title, date }) => {
  return (
    <div className="">
      <h1 className="text-2xl font-bold">{title}</h1>
      <span className="text-sm text-gray-500">{date ?? ""}</span>
    </div>
  );
};

export default ProfileNavTitle;
