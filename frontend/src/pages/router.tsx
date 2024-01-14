import { Route, Routes } from "react-router-dom";
import { routerType } from "../types/Router";
import pagesData from "./pagesData";
import Layout from "./Layout";

const Router = () => {
  const pageRoutes = pagesData.map(({ path, title, element }: routerType) => {
    return <Route key={title} path={`/${path}`} element={element} />;
  });

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
