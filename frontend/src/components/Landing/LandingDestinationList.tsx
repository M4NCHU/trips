import { FC, useState } from "react";
import { DestinationFilter, useDestinationList } from "src/api/Destinations";
import { BsCart } from "react-icons/bs";
import { ButtonWithIcon } from "../ui/Buttons/ButtonWithIcon";
import LandingSectionTitle from "./LandingSectionTitle";
import LandingItemsLayout from "./LandingItemsLayout";
import { Button } from "../ui/button";
import ItemsGrid from "../ui/ItemsGrid";
import CardItem from "../ui/CardItem";

interface LandingDestinationListProps {}

const LandingDestinationList: FC<LandingDestinationListProps> = () => {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const filter: DestinationFilter = {};
  if (selectedCategory) {
    filter.categoryId = selectedCategory;
  }
  const {
    data: destinations,
    isLoading,
    isError: DestinationsError,
  } = useDestinationList(filter);

  console.log(destinations);

  return (
    <LandingItemsLayout>
      <LandingSectionTitle
        title="Top Destinations"
        desc="lorem ipsuasd fasd fas df asdfasdf"
        dataCount={destinations.length ? destinations.length : undefined}
      />
      <div className="flex flex-row gap-2 flex-wrap">
        <Button
          className="bg-secondary rounded-full text-sm md:text-base text-primary hover:bg-secondary/80"
          onClick={() => setSelectedCategory(null)}
        >
          All
        </Button>
        {destinations
          ? destinations.map((item, i) => (
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

      <ItemsGrid
        items={destinations || []}
        renderItem={(item, index) => (
          <CardItem
            key={index}
            imageUrl={item.photoUrl}
            title={item.name}
            description={item.description}
            price="12 zł"
            actions={
              <ButtonWithIcon icon={<BsCart />} className="p-2 rounded-full" />
            }
          />
        )}
      />
    </LandingItemsLayout>
  );
};

export default LandingDestinationList;
