import { Link } from "react-router-dom";
import Logo from "../../assets/images/logo.png";

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
import NavLink from "./NavLink";
import { MdTravelExplore } from "react-icons/md";

const Header = () => {
  const { user, logout } = useAuth();

  const { data: UserTripsCount } = UseUserTripsCount(user?.id, 0);

  return (
    <>
      <nav className="w-full flex justify-between border-b-[1px] px-0 md:px-16 border-secondary">
        <div className="p-2 flex flex-row justify-between container gap-4">
          <Link
            to={"/"}
            className="flex flex-row items-center nav-logo bg-secondary px-4 rounded-full"
          >
            <img src={Logo} alt="site logo" className="w-12 " />
            <span className="text-xl font-bold">Travel</span>
          </Link>
          <div className="hidden md:flex flex-row bg-secondary grow rounded-full justify-between">
            <ul className="hidden md:flex flex-row  gap-4 bg-secondary rounded-full justify-between">
              <div className="flex flex-row ">
                <NavLink link="/accommodations" title="Accommodation" />
                <NavLink link="/destinations" title="Trips" />
                <NavLink link="/info" title="Information" />
              </div>
            </ul>
            <SearchModal />
          </div>

          <div className="user-panel flex items-center gap-2 text-sm md:text-2xl font-bold px-1 bg-secondary rounded-full">
            {UserTripsCount && UserTripsCount > 0 ? (
              <ButtonWithIcon
                icon={<MdTravelExplore />}
                className="bg-pink-600 rounded-full p-2 text-white hover:text-black text-2xl"
              />
            ) : (
              <ButtonWithIcon
                icon={<PiPlusCircle />}
                className="bg-pink-600 rounded-full p-2 text-white hover:text-black text-2xl"
              />
            )}

            <div className="hidden md:block">
              <Switcher />
            </div>
            <button className="p-1 hidden md:block">
              <PiHeartLight className="cursor-pointer" />
            </button>

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
          </div>
        </div>
      </nav>
      <BottomNavigation />
    </>
  );
};

export default Header;
