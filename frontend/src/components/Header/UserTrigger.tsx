import { FC } from "react";
import Logo from "../../assets/images/logo.png";

interface UserTriggerProps {}

const UserTrigger: FC<UserTriggerProps> = ({}) => {
  return (
    <div className="w-12 h-12 flex items-center">
      <img src={Logo} alt="site logo" className="object-contain" />
    </div>
  );
};

export default UserTrigger;
