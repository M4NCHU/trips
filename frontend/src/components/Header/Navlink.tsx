import { FC } from "react";
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
      className=" hover:bg-pink-600 gap-[1rem] flex items-center rounded-lg h-full justify-center p-0 md:p-[1rem] md:justify-start"
    >
      <div className="text-3xl font-semibold">{icon && icon}</div>
      <span className="hidden xl:inline-block text-xl font-semibold">
        {title}
      </span>
      {children && children}
    </Link>
  );
};

export default NavLink;
