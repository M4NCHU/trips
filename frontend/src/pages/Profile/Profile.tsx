import { FC } from "react";
import ProfileNav from "src/components/Profile/ProfileNav/ProfileNav";

interface ProfileProps {}

const Profile: FC<ProfileProps> = ({}) => {
  return (
    <div>
      <ProfileNav />
      Profil
    </div>
  );
};

export default Profile;
