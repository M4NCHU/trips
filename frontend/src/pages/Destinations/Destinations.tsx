import { FC, useState } from "react";
import { BsCart } from "react-icons/bs";
import { UseCategoryList } from "src/api/Category";
import { DestinationFilter, useDestinationList } from "src/api/Destinations";
import ProductsFilter from "src/components/Products/ProductsFilter";
import ProductsItem from "src/components/Products/ProductsItem";
import ProductsItemsList from "src/components/Products/ProductsItemsList";

import ProductsItemsSection from "src/components/Products/ProductsItemsSection";
import ProductsLayout from "src/components/Products/ProductsLayout";
import { ButtonWithIcon } from "src/components/ui/Buttons/ButtonWithIcon";
import { Button } from "src/components/ui/button";

interface DestinationsProps {}

const Destinations: FC<DestinationsProps> = ({}) => {
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [filter, setFilter] = useState<DestinationFilter>({});

  const applyFilters = () => {
    const newFilter: DestinationFilter = {};
    if (selectedCategory) {
      newFilter.categoryId = selectedCategory;
    }
    setFilter(newFilter);
  };
  const { data: destination, isError } = useDestinationList(filter);
  const {
    data: categories,
    isLoading,
    isError: CategoriesError,
  } = UseCategoryList();

  return (
    <ProductsLayout>
      <ProductsFilter>
        <div className="flex flex-row gap-2 flex-wrap">
          <Button
            className={`${
              selectedCategory == null ? "bg-pink-500" : "bg-background"
            } rounded-full text-sm md:text-sm text-primary hover:bg-secondary/80`}
            onClick={() => setSelectedCategory(null)}
          >
            All
          </Button>
          {categories
            ? categories.map((item, i) => (
                <Button
                  key={item.id}
                  className={`${
                    selectedCategory == item.id
                      ? "bg-pink-500"
                      : "bg-background"
                  } rounded-full text-sm md:text-sm text-primary hover:bg-secondary/80`}
                  onClick={() => setSelectedCategory(item.id)}
                >
                  {item.name}
                </Button>
              ))
            : ""}
        </div>
        <Button
          className="ml-2 p-2 bg-background hover:bg-background/80 text-white rounded-md"
          onClick={applyFilters}
        >
          Filtruj
        </Button>
      </ProductsFilter>
      <ProductsItemsSection>
        <ProductsItemsList>
          {destination.map((item, i) => (
            <ProductsItem link={`/destination/${item.id}`} key={i}>
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
            </ProductsItem>
          ))}
        </ProductsItemsList>
      </ProductsItemsSection>
    </ProductsLayout>
  );
};

export default Destinations;
