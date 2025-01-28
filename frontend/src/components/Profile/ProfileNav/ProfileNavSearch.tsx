import { FC } from "react";

interface ProfileNavSearchProps {}

const ProfileNavSearch: FC<ProfileNavSearchProps> = ({}) => {
  return (
    <input
      className="bg-secondary rounded-full hidden md:block px-4 py-2"
      placeholder="Search for ... "
    />
  );
};

export default ProfileNavSearch;
