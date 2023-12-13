import React from "react";
import { Link } from "react-router-dom";
import Logo from "../../assets/images/logo.png";

import { CiSun } from "react-icons/ci";
import { PiHeartLight } from "react-icons/pi";

const Header = () => {
  return (
    <nav className="w-full flex justify-center">
      <div className="p-2 flex flex-row justify-between container">
        <div className="nav-logo ">
          <div className="w-12 h-12 flex items-center">
            <img src={Logo} alt="site logo" className="object-contain" />
          </div>
        </div>
        <ul className="flex flex-row items-center gap-4 ">
          <li>
            <Link to="/">Home</Link>
          </li>
          <li>
            <Link to="/blogs">Blogs</Link>
          </li>
          <li>
            <Link to="/contact">Contact</Link>
          </li>
        </ul>
        <div className="user-panel flex items-center gap-2 text-2xl font-bold">
          <CiSun className="cursor-pointer" />
          <PiHeartLight className="cursor-pointer" />
          <div className="w-12 h-12 flex items-center">
            <img src={Logo} alt="site logo" className="object-contain" />
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Header;
