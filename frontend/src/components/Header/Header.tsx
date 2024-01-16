import React from "react";
import { Link } from "react-router-dom";
import Logo from "../../assets/images/logo.png";

import { CiSun } from "react-icons/ci";
import { PiHeartLight } from "react-icons/pi";
import Navlink from "./Navlink";
import SearchModal from "../Modals/SearchModal";
import Switcher from "../ui/Switcher";
import { Button } from "../ui/button";

const Header = () => {
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
              2
            </div>
            Planning
          </Link>
          <Switcher />
          <button>
            <PiHeartLight className="cursor-pointer" />
          </button>
          <div className="w-12 h-12 flex items-center">
            <img src={Logo} alt="site logo" className="object-contain" />
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Header;
