import { FC } from "react";
import { CiHome } from "react-icons/ci";
import { Link } from "react-router-dom";

interface BNavLinkProps {
  text?: string;
  link: string;
  icon: React.ReactNode;
}

const BNavLink: FC<BNavLinkProps> = ({ text, link, icon }) => {
  return (
    <Link to={link} className="p-2">
      {icon}
    </Link>
  );
};

export default BNavLink;
