import { Route, Routes } from "react-router-dom";
import { routerType } from "../types/Router";
import Layout from "./Layout";
import pagesData from "./pagesData";
import ProtectedRoute from "./ProtectedRoute";
import AdminLayout from "./AdminLayout";

const Router = () => {
  const pageRoutes = pagesData.map(
    ({ path, title, element, isProtected, roles, isAdminPage }: routerType) => {
      const routeElement = isProtected ? (
        <ProtectedRoute roles={roles}>{element}</ProtectedRoute>
      ) : (
        element
      );

      // Owrapowanie komponentu w odpowiedni layout
      const wrappedRouteElement = isAdminPage ? (
        <AdminLayout>{routeElement}</AdminLayout>
      ) : (
        <Layout>{routeElement}</Layout>
      );

      return (
        <Route key={title} path={`/${path}`} element={wrappedRouteElement} />
      );
    }
  );

  return <Routes>{pageRoutes}</Routes>;
};

export default Router;
