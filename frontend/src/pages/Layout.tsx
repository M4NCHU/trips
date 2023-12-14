import { Outlet } from "react-router-dom";
import Header from "../components/Header/Header";
import CategoriesList from "../components/Categories/CategoriesList";

const Layout = () => {
  return (
    <>
      <Header />

      <Outlet />
    </>
  );
};

export default Layout;
