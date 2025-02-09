import { Link } from "react-router-dom";

import { PiHeartLight, PiPlusCircle } from "react-icons/pi";
import { useAuth } from "src/context/UserContext";
import SearchModal from "../Modals/SearchModal";
import Switcher from "../ui/Switcher";

import { TbLogout2 } from "react-icons/tb";
import { UseUserTripsCount } from "src/api/TripAPI";
import BottomNavigation from "../BottomNavigation/BottomNavigation";
import { ButtonWithIcon } from "../ui/Buttons/ButtonWithIcon";
import CustomDropdownMenu from "../ui/Dropdown/CustomDropdownMenu";
import CustomDropdownMenuItem from "../ui/Dropdown/CustomDropdownMenuItem";
import UserTrigger from "./UserTrigger";
import NavLink from "./Navlink";
import { MdTravelExplore } from "react-icons/md";

const Header = () => {
  const { isAuthenticated, user, logout } = useAuth();
  return (
    <>
      <nav className="w-full flex justify-between px-0 bg-background">
        <div className="pt-4 pl-2 pr-6 flex flex-row justify-between gap-4 w-full">
          <SearchModal />
          <div className="user-panel flex items-center gap-[1rem] text-sm md:text-2xl font-bold  rounded-full">
            <button className="p-2 rounded-lg hover:bg-pink-600 hidden md:block">
              <PiHeartLight className="cursor-pointer" />
            </button>
            <Link
              to={"/cart"}
              className="px-4 py-2 font-semibold text-base bg-pink-600 hover:bg-pink-700 rounded-lg"
            >
              Reservations
            </Link>
            {isAuthenticated() && user ? (
              <CustomDropdownMenu dropDownButton={<UserTrigger />}>
                <div className="bg-background rounded-lg p-2 text-base">
                  <span>{user.lastName}</span>
                </div>
                <CustomDropdownMenuItem label="Profile" href="/profile" />
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
          </div>
        </div>
      </nav>
      <BottomNavigation />
    </>
  );
};

export default Header;
