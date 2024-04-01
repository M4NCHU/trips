import { FC } from "react";
import { Link } from "react-router-dom";

interface NavLinkProps {
  link: string;
  children?: React.ReactNode;
  title?: string;
}

const NavLink: FC<NavLinkProps> = ({ link, title, children }) => {
  return (
    <Link
      to={link}
      className="px-6 hover:bg-pink-600 flex items-center rounded-full h-full"
    >
      {title && title}
      {children && children}
    </Link>
  );
};

export default NavLink;
