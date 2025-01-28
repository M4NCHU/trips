import { FC, useState } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header/Header";
import Sidebar from "src/components/Sidebar/Sidebar";
import AdminNav from "src/components/Admin/AdminNav/AdminNav";

interface LayoutProps {
  isAdmin?: boolean;
  children: React.ReactNode;
}

const Layout: FC<LayoutProps> = ({ isAdmin, children }) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  return (
    <div className="min-h-screen flex flex-col overflow-hidden">
      <div className="flex flex-col lg:flex-row w-full grow">
        <div className="hidden lg:block lg:w-20 xl:w-[20rem]">
          <Sidebar />
        </div>

        <div className="flex flex-col h-screen grow w-full lg:w-[calc(100vw-5rem)] xl:w-[calc(100vw-20rem)]">
          <Header />
          <div className="rounded-2xl m-[.5rem] md:m-[1rem] flex flex-col bg-secondary text-centralSection-foreground grow overflow-y-auto gap-2 p-[2rem] max-h-[calc(100vh-66px)]">
            {isAdmin && <AdminNav />}
            {children}
          </div>
        </div>

        <button
          onClick={toggleSidebar}
          className="lg:hidden p-2 bg-gray-800 text-white fixed bottom-4 right-4 rounded-full"
        >
          Toggle Sidebar
        </button>
      </div>

      <Outlet />
    </div>
  );
};

export default Layout;
