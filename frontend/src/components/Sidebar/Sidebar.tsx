import React from "react";
import { Link } from "react-router-dom";
import Logo from "../../assets/images/logo.png";
import NavLink from "../Header/Navlink";
import { CiSettings } from "react-icons/ci";
import { MdOutlineHotel, MdOutlineLocationOn } from "react-icons/md";
import { IoMdInformationCircleOutline } from "react-icons/io";
import Switcher from "../ui/Switcher";

const Sidebar = () => {
  return (
    <aside className="h-screen bg-background text-primary lg:w-20 xl:w-[20rem] transition-all duration-300">
      <div className="flex flex-col justify-between h-full p-4">
        <div className="sidebar-logo border-b-[1px] w-full border-secondary mb-2 flex items-center justify-center xl:justify-center">
          <Link
            to="/"
            className="flex items-center nav-logo hover:bg-secondary w-full rounded-lg mb-2"
          >
            <img
              src={Logo}
              alt="site logo"
              className="w-16 xl:w-24 transition-all"
            />
            <span className="text-2xl font-bold hidden xl:block ml-2">
              Travel
            </span>
          </Link>
        </div>

        <div className="grow py-4">
          <ul className="flex flex-col gap-4 mt-4">
            <NavLink
              link="/accommodations"
              title="Accommodations"
              icon={<MdOutlineHotel />}
            />
            <NavLink
              link="/destinations"
              title="Trips"
              icon={<MdOutlineLocationOn />}
            />
          </ul>
        </div>

        <div className="sidebar-footer w-full bg-secondary rounded-xl py-3 px-4 flex items-center justify-center xl:justify-between">
          <div className="flex flex-col xl:flex-row gap-4 items-center text-2xl">
            <Switcher />
            <CiSettings className="" />
          </div>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
