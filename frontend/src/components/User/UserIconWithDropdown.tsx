import { FC } from "react";
import { useAuth } from "src/context/UserContext";
import CustomDropdownMenu from "../ui/Dropdown/CustomDropdownMenu";
import UserTrigger from "../Header/UserTrigger";
import CustomDropdownMenuItem from "../ui/Dropdown/CustomDropdownMenuItem";
import { TbLogout2 } from "react-icons/tb";
import { Link } from "react-router-dom";

interface UserIconWithDropdownProps {}

const UserIconWithDropdown: FC<UserIconWithDropdownProps> = ({}) => {
  const { user, logout } = useAuth();

  return (
    <>
      {user ? (
        <CustomDropdownMenu dropDownButton={<UserTrigger />}>
          <div className="bg-background rounded-lg p-2 text-base">
            <span>{user.lastName}</span>
          </div>
          <CustomDropdownMenuItem label="Home" href="/" />
          <CustomDropdownMenuItem label="Contact" href="/contact" />
          <CustomDropdownMenuItem label="About" href="/about" />
          <CustomDropdownMenuItem label="Admin" href="/admin" />
          <div className="my-2 h-[1px] bg-gray-800"></div>
          <CustomDropdownMenuItem
            label="Logout"
            variant="danger"
            onClick={logout}
            icon={<TbLogout2 />}
          />
        </CustomDropdownMenu>
      ) : (
        <Link
          to={"/login"}
          className="px-4 py-2 font-semibold text-base bg-secondary rounded-lg"
        >
          Login
        </Link>
      )}
    </>
  );
};

export default UserIconWithDropdown;
