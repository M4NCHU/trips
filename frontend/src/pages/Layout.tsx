import { FC } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header/Header";

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  return (
    <>
      <Header />
      {children}
      <Outlet />
    </>
  );
};

export default Layout;
