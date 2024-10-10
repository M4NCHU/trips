import { FC } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header/Header";
import Sidebar from "src/components/Sidebar/Sidebar";
import AdminNav from "src/components/Admin/AdminNav/AdminNav";

interface LayoutProps {
  isAdmin?: boolean;
  children: React.ReactNode;
}

const Layout: FC<LayoutProps> = ({ isAdmin, children }) => {
  return (
    <div className="min-h-screen flex flex-col">
      <div className="flex flex-col lg:flex-row w-full grow">
        <Sidebar />

        <div className="w-full flex flex-col h-screen">
          <Header />
          <div className="p-4 rounded-2xl m-[.5rem] md:m-[1rem] flex flex-col bg-secondary text-centralSection-foreground grow px-[.2rem] md:px-[2rem] overflow-y-auto gap-2">
            {isAdmin && <AdminNav />}
            {children}
          </div>
        </div>

        <button className="lg:hidden p-2 bg-gray-800 text-white fixed bottom-4 right-4 rounded-full">
          Toggle Sidebar
        </button>
        <div
          className="lg:hidden fixed top-0 left-0 w-64 h-full bg-gray-800 text-white p-4 transform -translate-x-full transition-transform duration-300 ease-in-out"
          id="mobileSidebar"
        >
          <Sidebar />
        </div>
      </div>

      <Outlet />
    </div>
  );
};

export default Layout;
