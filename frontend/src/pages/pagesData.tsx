import DestinationDetails from "../components/Destinations/DestinationDetails";
import { routerType } from "../types/Router";
import Login from "./Authentication/Login/Login";
import Register from "./Authentication/Register/Register";
import CreateCategory from "./Categories/Create";
import Contact from "./Contact/Contact";
import CreateDestination from "./Destinations/Create";
import Home from "./Home/Home";
import CreateVisitPlace from "./VisitPlaces/Create";

const pagesData: routerType[] = [
  {
    path: "",
    element: <Home />,
    title: "home",
  },
  {
    path: "contact",
    element: <Contact />,
    title: "contact",
  },
  {
    path: "destinations/create",
    element: <CreateDestination />,
    title: "Create trip destination",
  },
  {
    path: "destinations/details/:id",
    element: <DestinationDetails />,
    title: "Destination Details",
  },
  {
    path: "categories/create",
    element: <CreateCategory />,
    title: "Create destination category",
  },
  {
    path: ":id/visit-place/create",
    element: <CreateVisitPlace />,
    title: "Create visit place",
  },
  {
    path: "login",
    element: <Login />,
    title: "login",
  },
  {
    path: "register",
    element: <Register />,
    title: "register",
  },
];

export default pagesData;
