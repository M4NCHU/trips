import { Route, Routes } from "react-router-dom";
import { routerType } from "../types/Router";
import Layout from "./Layout";
import pagesData from "./pagesData";
import ProtectedRoute from "./ProtectedRoute";

const Router = () => {
  const pageRoutes = pagesData.map(
    ({ path, title, element, isProtected, roles }: routerType) => {
      const routeElement = isProtected ? (
        <ProtectedRoute roles={roles}>{element}</ProtectedRoute>
      ) : (
        element
      );
      return <Route key={title} path={`/${path}`} element={routeElement} />;
    }
  );
  return (
    <Routes>
      {" "}
      <Route path="/" element={<Layout />}>
        {pageRoutes}
      </Route>
    </Routes>
  );
};

export default Router;
