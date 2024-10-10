import { FC } from "react";
import { CiHome } from "react-icons/ci";
import { Link } from "react-router-dom";

interface NavLinkProps {
  link: string;
  children?: React.ReactNode;
  title?: string;
  icon?: React.ReactNode;
}

const NavLink: FC<NavLinkProps> = ({ link, title, icon, children }) => {
  return (
    <Link
      to={link}
      className="px-6 hover:bg-pink-600 py-[1rem] gap-[1rem] flex items-center rounded-lg h-full"
    >
      <div className="text-3xl font-semibold">{icon && icon}</div>
      <span className="text-xl font-semibold">{title && title}</span>
      {children && children}
    </Link>
  );
};

export default NavLink;
