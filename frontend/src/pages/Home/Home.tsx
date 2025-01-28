import LandingButton from "src/components/Landing/LandingButton";
import Image from "../../assets/images/vecteezy_travel-around-the-world-important-landmarks-poster_1128259.jpg";
import LandingSectionTitle from "src/components/Landing/LandingSectionTitle";
import LandingItemsSection from "src/components/Landing/LandingItemsSection";
import LandingCategoriesList from "src/components/Landing/LandingCategoriesList";
import LandingDestinationList from "src/components/Landing/LandingDestinationList";
import CategorySlider from "src/components/Categories/Lists/CategorySlider";

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
      {/* <div className="px-[.2rem] lg:px-[1rem] mx-0 flex justify-center pb-2 lg:pb-4 overflow-x-auto min-h-[10rem] w-full">
        <CategorySlider />
      </div> */}
      <div
        className="w-full min-h-[18rem] h-[18rem] md:h-[28rem] rounded-lg mt-4 bg-cover bg-center relative"
        style={{ backgroundImage: `url(${Image})` }}
      ></div>

      <div className="flex flex-col mt-12 gap-12">
        <LandingDestinationList />
        <LandingCategoriesList />
      </div>
    </>
  );
};

export default Home;
