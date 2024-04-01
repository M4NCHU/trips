import LandingButton from "src/components/Landing/LandingButton";
import Image from "../../assets/images/test.webp";
import LandingSectionTitle from "src/components/Landing/LandingSectionTitle";
import LandingItemsSection from "src/components/Landing/LandingItemsSection";
import LandingCategoriesList from "src/components/Landing/LandingCategoriesList";
import LandingDestinationList from "src/components/Landing/LandingDestinationList";

type DestinationType = {
  id: string;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
};

const Home = () => {
  return (
    <div className="container px-4">
      <div className="flex flex-col justify-center items-center py-12 gap-4">
        <h1 className="text-2xl md:text-4xl font-semibold">
          Travel Smarter, Dream bigger
        </h1>
        <p className="text-base md:text-lg">
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit
          assumenda consequatur
        </p>
        <div className="flex flex-row items-center gap-4">
          <LandingButton text="Plan your trip" link="/destinations" />
          <LandingButton text="Accommodations" link="/" />
        </div>
        <div
          className="w-full h-[18rem] md:h-[24rem] rounded-lg mt-4 bg-cover bg-center relative"
          style={{ backgroundImage: `url(${Image})` }}
        ></div>
      </div>
      <div className="flex flex-col mt-12 gap-12">
        <LandingDestinationList />

        <LandingCategoriesList />
      </div>
      {/* <CategoriesList />
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
      </div> */}
    </div>
  );
};

export default Home;
