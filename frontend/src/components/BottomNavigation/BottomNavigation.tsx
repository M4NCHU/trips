import { FC, useState, useEffect } from "react";
import { GoHome } from "react-icons/go";
import BNavLink from "./BNavLink"; // Make sure this is your import
import { FaRegHeart } from "react-icons/fa";
import Switcher from "../ui/Switcher"; // Make sure this is your import
import { FiPhone } from "react-icons/fi";
import SearchModal from "../Modals/SearchModal";

interface BottomNavigationProps {}

const BottomNavigation: FC<BottomNavigationProps> = () => {
  const [visible, setVisible] = useState(true);
  const [lastScrollY, setLastScrollY] = useState(0);

  const controlNavbar = () => {
    if (typeof window !== "undefined") {
      // Checks if window object is available
      setVisible(window.scrollY < lastScrollY || window.scrollY === 0);
      setLastScrollY(window.scrollY);
    }
  };

  useEffect(() => {
    window.addEventListener("scroll", controlNavbar);
    return () => window.removeEventListener("scroll", controlNavbar);
  }, [lastScrollY]);

  return (
    <div
      className={`w-full px-5 py-2 fixed bottom-0 sm:hidden bg-secondary z-[9999] transition-transform duration-300 ${
        visible ? "translate-y-0" : "translate-y-full"
      }`}
    >
      <ul className="bottom-nav flex flex-row justify-between items-center gap-2 text-2xl flex-wrap">
        <BNavLink icon={<Switcher />} link={"/"} />
        <BNavLink icon={<FaRegHeart />} link={"/"} />
        <BNavLink icon={<FiPhone />} link={"/"} />
        <SearchModal />
      </ul>
    </div>
  );
};

export default BottomNavigation;
