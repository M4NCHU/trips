import { GoPlus } from "react-icons/go";
import { Link } from "react-router-dom";
import CategoriesList from "../../components/Categories/CategoriesList";
import DestinationsList from "../../components/Destinations/DestinationsList";
import Card from "../../components/Cards/CardTest";

type DestinationType = {
  id: string;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
};

const Home = () => {
  return (
    <>
      <CategoriesList />
      <div className="container my-5 px-4">
        <div className="hidden sm:flex justify-end fixed bottom-4 right-4 z-[9999]">
          <Link
            to="/destinations/create"
            className="bg-secondary p-5 text-xl font-bold rounded-full"
          >
            <GoPlus />
          </Link>
        </div>
        <DestinationsList />
      </div>
    </>
  );
};

export default Home;
