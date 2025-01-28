import { FC } from "react";
import ProfileNavTitle from "./ProfileNavTitle";
import ProfileLink from "./ProfileLink";

interface ProfileNavProps {}

const ProfileNav: FC<ProfileNavProps> = ({}) => {
  return (
    <div className="flex flex-row justify-between py-2 border-b-[1px] border-background">
      <ProfileNavTitle title="Profile" />
      <div className="flex flex-row items-center gap-2 py-2">
        <ul className="profile-link-ul flex flex-row">
          <ProfileLink link={"/profile/info"} title={"Informations"} />
          <ProfileLink link={"/profile/reservations"} title={"Reservations"} />
        </ul>
      </div>
    </div>
  );
};

export default ProfileNav;
