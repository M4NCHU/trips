import { FC } from "react";

interface NavlinkProps {
  children: React.ReactNode;
}

const Navlink: FC<NavlinkProps> = ({ children }) => {
  return <li className="cursor-pointer">{children}</li>;
};

export default Navlink;
