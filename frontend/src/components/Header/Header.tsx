import { Link } from "react-router-dom";
import Logo from "../../assets/images/logo.png";

import { PiHeartLight } from "react-icons/pi";
import SearchModal from "../Modals/SearchModal";
import Switcher from "../ui/Switcher";
import Navlink from "./Navlink";
import { UseTripDestinationCount } from "src/api/TripDestinationAPI";
import { useAuth } from "src/context/UserContext";

import CustomDropdownMenu from "../ui/Dropdown/CustomDropdownMenu";
import CustomDropdownMenuItem from "../ui/Dropdown/CustomDropdownMenuItem";
import UserTrigger from "./UserTrigger";
import { TbLogout2 } from "react-icons/tb";
import { UseUserTripsCount } from "src/api/TripAPI";

const Header = () => {
  const { user, logout } = useAuth();

  const { data: UserTripsCount } = UseUserTripsCount(user?.id, 0);

  return (
    <nav className="w-full flex justify-center border-b-[1px] border-secondary">
      <div className="p-2 flex flex-row justify-between container">
        <div className="nav-logo ">
          <Link to={"/"} className="w-12 h-12 flex items-center">
            <img src={Logo} alt="site logo" className="object-contain" />
          </Link>
        </div>
        <ul className="hidden md:flex flex-row items-center gap-4 ">
          <Navlink>
            <Link to="/">Home</Link>
          </Navlink>

          <Navlink>
            <Link to="/blogs">Blogs</Link>
          </Navlink>

          <Navlink>
            <Link to="/contact">Contact</Link>
          </Navlink>

          <Navlink>
            <SearchModal />
          </Navlink>
        </ul>
        <div className="user-panel flex items-center gap-4 text-2xl font-bold">
          <Link
            to={`/planning`}
            className="bg-pink-600 p-2 rounded-lg text-base text-white relative"
          >
            <div className="absolute -top-2 -right-2 bg-green-400 rounded-full p-1 w-6 h-6 flex items-center justify-center">
              {UserTripsCount}
            </div>
            Planning
          </Link>
          <Switcher />
          <button className="p-1">
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
  );
};

export default Header;
