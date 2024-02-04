import DestinationDetails from "../components/Destinations/DestinationDetails";
import { routerType } from "../types/Router";
import Login from "./Authentication/Login/Login";
import Register from "./Authentication/Register/Register";
import UseCreateCategory from "./Categories/Create";
import Contact from "./Contact/Contact";
import CreateDestination from "./Destinations/Create";
import Home from "./Home/Home";
import Planning from "./Planning/Planning";
import CreateVisitPlace from "./VisitPlaces/Create";

const pagesData: routerType[] = [
  {
    path: "",
    element: <Home />,
    title: "home",
    isProtected: false,
  },
  {
    path: "contact",
    element: <Contact />,
    title: "contact",
    isProtected: false,
  },
  {
    path: "destinations/create",
    element: <CreateDestination />,
    title: "Create trip destination",
    isProtected: false,
  },
  {
    path: "destination/:id",
    element: <DestinationDetails />,
    title: "Destination Details",
    isProtected: false,
  },
  {
    path: "categories/create",
    element: <UseCreateCategory />,
    title: "Create destination category",
    isProtected: false,
  },
  {
    path: "destination/:id/visit-place/create",
    element: <CreateVisitPlace />,
    title: "Create visit place",
    isProtected: false,
  },
  {
    path: "planning/:id",
    element: <Planning />,
    title: "Plan your trip",
    isProtected: true,
  },
  {
    path: "login",
    element: <Login />,
    title: "login",
    isProtected: false,
  },
  {
    path: "register",
    element: <Register />,
    title: "register",
    isProtected: false,
  },
];

export default pagesData;
