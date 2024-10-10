import React from "react";
import { Link } from "react-router-dom";
import Logo from "../../assets/images/logo.png";
import NavLink from "../Header/Navlink";
import { CiSettings } from "react-icons/ci";
import { MdOutlineHotel, MdOutlineLocationOn } from "react-icons/md";
import { IoMdInformationCircleOutline } from "react-icons/io";

const Sidebar = () => {
  return (
    <aside className="w-full lg:w-1/5 bg-background text-primary h-screen lg:block hidden min-w-[20rem]">
      <div className="flex flex-col justify-between h-screen p-4">
        <div className="sidebar-logo border-b-[1px] border-secondary">
          <Link
            to={"/"}
            className="flex flex-row items-center py-[1rem] nav-logo hover:bg-secondary mb-2 px-4 rounded-lg"
          >
            <img src={Logo} alt="site logo" className="w-24 " />
            <span className="text-2xl font-bold hidden lg:block">Travel</span>
          </Link>
        </div>
        <div className="grow py-[4rem]">
          <ul className="flex flex-col gap-4">
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
            <NavLink
              link="/info"
              title="Information"
              icon={<IoMdInformationCircleOutline />}
            />
          </ul>
        </div>

        <div className="sidebar-footer w-full bg-secondary rounded-xl py-[1rem] px-[.6rem] flex flex-row justify-between">
          <div>user</div>
          <div className="sidebar-settings">
            <CiSettings className="" />
          </div>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
