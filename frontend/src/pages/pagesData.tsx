import { routerType } from "../types/Router";
import Accommodations from "./Accommodations/Accomodations";
import DetailsAccomodation from "./Accommodations/Details";
import Admin from "./Admin/Admin";
import CategoryAdmin from "./Admin/Categories/Categories";
import CreateDestination from "./Admin/Destinations/Create";
import DestinationsAdmin from "./Admin/Destinations/Destinations";
import Login from "./Authentication/Login/Login";
import Register from "./Authentication/Register/Register";
import Cart from "./Cart/Cart";
import UseCreateCategory from "./Categories/Create";
import Contact from "./Contact/Contact";
import Destinations from "./Destinations/Destinations";
import Details from "./Destinations/Details";
import Home from "./Home/Home";
import ChooseTripScheme from "./Planning/ChooseTripScheme";
import Planning from "./Planning/Planning";
import Profile from "./Profile/Profile";
import Reservation from "./Profile/Reservations/Reservation";
import Reservations from "./Profile/Reservations/Reservations";
import Resume from "./Resume/Resume";
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
    element: <Details />,
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
    path: "accommodations",
    element: <Accommodations />,
    title: "Accommodations",
    isProtected: false,
  },

  {
    path: "accommodation/:id",
    element: <DetailsAccomodation />,
    title: "Accommodation Details",
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
    path: "cart",
    element: <Cart />,
    title: "Cart",
    isProtected: false,
  },

  {
    path: "resume",
    element: <Resume />,
    title: "Resume",
    isProtected: false,
  },

  {
    path: "profile",
    element: <Profile />,
    title: "Profile",
    isProtected: true,
  },

  {
    path: "profile/reservations",
    element: <Reservations />,
    title: "Reservations",
    isProtected: true,
  },

  {
    path: "profile/reservations/:id",
    element: <Reservation />,
    title: "Reservation details",
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
