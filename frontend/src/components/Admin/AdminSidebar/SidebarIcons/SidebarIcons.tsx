import { FC } from "react";
import logo from "../../../../assets/images/logo.png";
import { MdHolidayVillage } from "react-icons/md";
import { Link } from "react-router-dom";

interface SidebarIconsProps {}

const SidebarIcons: FC<SidebarIconsProps> = ({}) => {
  return (
    <div className=" py-2 pl-4 pr-2 flex flex-col items-center gap-4">
      <div className="sidebar-icons__logo p-1 flex flex-row w-16 h-16 justify-center">
        <img src={logo} alt="Admin sidebar logo" className="w-12 h-12" />
      </div>

      <div className="sidebar-icons__icons p-1 flex flex-row">
        <ul className="flex flex-col justify-center ">
          <li className="flex justify-center">
            <Link to={"/"} className="hover:bg-secondary p-2 rounded-xl">
              <MdHolidayVillage className="text-4xl text-gray-400" />
            </Link>
          </li>
          <li className="flex justify-center">
            <Link to={"/"} className="hover:bg-secondary p-2 rounded-xl">
              <MdHolidayVillage className="text-4xl text-gray-400" />
            </Link>
          </li>

          <li className="flex justify-center">
            <Link to={"/"} className="hover:bg-secondary p-2 rounded-xl">
              <MdHolidayVillage className="text-4xl text-gray-400" />
            </Link>
          </li>
          <li className="flex justify-center">
            <Link to={"/"} className="hover:bg-secondary p-2 rounded-xl">
              <MdHolidayVillage className="text-4xl text-gray-400" />
            </Link>
          </li>
          <li className="flex justify-center">
            <Link to={"/"} className="hover:bg-secondary p-2 rounded-xl">
              <MdHolidayVillage className="text-4xl text-gray-400" />
            </Link>
          </li>
          <li>
            <Link to={"/"}>
              <MdHolidayVillage />
            </Link>
          </li>
        </ul>
      </div>
    </div>
  );
};

export default SidebarIcons;
