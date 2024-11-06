import DestinationDetails from "../components/Destinations/DestinationDetails";
import { routerType } from "../types/Router";
import Admin from "./Admin/Admin";
import CategoryAdmin from "./Admin/Categories/Categories";
import CreateDestination from "./Admin/Destinations/Create";
import DestinationsAdmin from "./Admin/Destinations/Destinations";
import Login from "./Authentication/Login/Login";
import Register from "./Authentication/Register/Register";
import UseCreateCategory from "./Categories/Create";
import Contact from "./Contact/Contact";
import Destinations from "./Destinations/Destinations";
import Home from "./Home/Home";
import ChooseTripScheme from "./Planning/ChooseTripScheme";
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
    path: "destinations",
    element: <Destinations />,
    title: "Destinations",
    isProtected: false,
  },

  {
    path: "destination/:id",
    element: <DestinationDetails />,
    title: "Destination Details",
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
    path: "planning",
    element: <ChooseTripScheme />,
    title: "Choose trip scheme",
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

  {
    path: "/admin",
    element: <Admin />,
    title: "home",
    isProtected: true,
    isAdminPage: true,
  },

  {
    path: "admin/destinations",
    element: <DestinationsAdmin />,
    title: "View admin destinations",
    isProtected: false,
    isAdminPage: true,
  },
  {
    path: "admin/destinations/create",
    element: <CreateDestination />,
    title: "Create trip destination",
    isProtected: false,
    isAdminPage: true,
  },

  {
    path: "admin/categories",
    element: <CategoryAdmin />,
    title: "Categories list",
    isProtected: false,
    isAdminPage: true,
  },

  {
    path: "admin/categories/create",
    element: <UseCreateCategory />,
    title: "Create destination category",
    isProtected: false,
    isAdminPage: true,
  },
];

export default pagesData;
