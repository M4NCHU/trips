import { FC, useState } from "react";
import Landing from "src/pages/Landing/Landing";
import LandingCardItem from "./LandingCardItem";
import LandingItemsSection from "./LandingItemsSection";
import { UseCategoryList } from "src/api/Category";
import { DestinationFilter, useDestinationList } from "src/api/Destinations";
import { PiCarThin } from "react-icons/pi";
import { BsCart } from "react-icons/bs";
import { ButtonWithIcon } from "../ui/Buttons/ButtonWithIcon";
import LandingSectionTitle from "./LandingSectionTitle";
import LandingItemsLayout from "./LandingItemsLayout";
import { Button } from "../ui/button";

interface LandingDestinationListProps {}

const LandingDestinationList: FC<LandingDestinationListProps> = ({}) => {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const filter: DestinationFilter = {};
  if (selectedCategory) {
    filter.categoryId = selectedCategory;
  }
  const { data: destination, isError } = useDestinationList(filter);
  const {
    data: categories,
    isLoading,
    isError: CategoriesError,
  } = UseCategoryList();

  return (
    <LandingItemsLayout>
      <LandingSectionTitle
        title="Top Destinations"
        desc="lorem ipsuasd fasd fas df asdfasdf"
        dataCount={destination.length ? destination.length : undefined}
      />
      <div className="flex flex-row gap-2 flex-wrap">
        <Button
          className="bg-secondary rounded-full text-sm md:text-base text-primary hover:bg-secondary/80"
          onClick={() => setSelectedCategory(null)}
        >
          All
        </Button>
        {categories
          ? categories.map((item, i) => (
              <Button
                key={item.id}
                className="bg-secondary rounded-full text-sm md:text-base text-primary hover:bg-secondary/80"
                onClick={() => setSelectedCategory(item.id)}
              >
                {item.name}
              </Button>
            ))
          : ""}
      </div>
      <LandingItemsSection>
        {destination
          ? destination.map((item, i) => (
              <LandingCardItem key={i}>
                <div className="flex flex-col gap-2">
                  <div className="w-full h-32 overflow-hidden">
                    <img
                      src={item.photoUrl}
                      alt=""
                      className="w-full h-full object-cover rounded-lg"
                    />
                  </div>
                  <div className="flex flex-col gap-2 p-2">
                    <h4 className="text-lg font-semibold">{item.name}</h4>
                    <p className="text-gray-400 text-sm">{item.description}</p>
                    <div className="flex flex-row  justify-between items-center">
                      <span className="font-bold">12 zł</span>
                      <ButtonWithIcon
                        icon={<BsCart />}
                        className="p-2 rounded-full"
                      />
                    </div>
                  </div>
                </div>
              </LandingCardItem>
            ))
          : "No destinations"}
      </LandingItemsSection>
    </LandingItemsLayout>
  );
};

export default LandingDestinationList;
