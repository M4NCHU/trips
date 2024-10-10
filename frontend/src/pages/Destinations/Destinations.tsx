import { FC, useState } from "react";
import { BsCart } from "react-icons/bs";
import { UseCategoryList } from "src/api/Category";
import { useDestinationList } from "src/api/Destinations";
import ProductsFilter from "src/components/Products/ProductsFilter";
import ProductsItem from "src/components/Products/ProductsItem";
import ProductsItemsList from "src/components/Products/ProductsItemsList";
import ProductsItemsSection from "src/components/Products/ProductsItemsSection";
import ProductsLayout from "src/components/Products/ProductsLayout";
import { ButtonWithIcon } from "src/components/ui/Buttons/ButtonWithIcon";
import { Button } from "src/components/ui/button";
import useFilter from "src/hooks/UseFilter";

interface DestinationsProps {}

const Destinations: FC<DestinationsProps> = () => {
  const { filters, updateFilter, resetFilters } = useFilter();

  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [minPrice, setMinPrice] = useState<number | null>(null);
  const [maxPrice, setMaxPrice] = useState<number | null>(null);
  const [minRating, setMinRating] = useState<number | null>(null);
  const [maxRating, setMaxRating] = useState<number | null>(null);

  const applyFilters = () => {
    updateFilter("categoryId", selectedCategory ?? undefined);
    updateFilter("minPrice", minPrice ?? undefined);
    updateFilter("maxPrice", maxPrice ?? undefined);
    updateFilter("minRating", minRating ?? undefined);
    updateFilter("maxRating", maxRating ?? undefined);
  };

  const { data: destination, isError } = useDestinationList(filters);
  const {
    data: categories,
    isLoading,
    isError: CategoriesError,
  } = UseCategoryList();

  return (
    <ProductsLayout>
      <ProductsFilter>
        {/* Filtry */}
        <div className="flex flex-row gap-2 flex-wrap">
          <Button onClick={() => setSelectedCategory(null)}>All</Button>
          {categories &&
            categories.map((item) => (
              <Button
                key={item.id}
                onClick={() => setSelectedCategory(item.id)}
                className={selectedCategory === item.id ? "bg-pink-500" : ""}
              >
                {item.name}
              </Button>
            ))}
        </div>

        {/* Filtry po cenie */}
        <div className="flex flex-col gap-2 mt-4">
          <label>
            Min Price:
            <input
              type="number"
              value={minPrice ?? ""}
              onChange={(e) => setMinPrice(Number(e.target.value) || null)}
            />
          </label>
          <label>
            Max Price:
            <input
              type="number"
              value={maxPrice ?? ""}
              onChange={(e) => setMaxPrice(Number(e.target.value) || null)}
            />
          </label>
        </div>

        {/* Przycisk do zastosowania filtrów */}
        <Button onClick={applyFilters}>Filtruj</Button>
      </ProductsFilter>

      <ProductsItemsSection>
        <ProductsItemsList>
          {destination.map((item, i) => (
            <ProductsItem link={`/destination/${item.id}`} key={i}>
              <div className="flex flex-col gap-2">
                <div className="w-full h-32 overflow-hidden">
                  <img
                    src={item.photoUrl}
                    alt="destination"
                    className="w-full h-full object-cover rounded-lg"
                  />
                </div>
                <div className="flex flex-col gap-2 p-2">
                  <h4 className="text-lg font-semibold">{item.name}</h4>
                  <p className="text-gray-400 text-sm">{item.description}</p>
                  <div className="flex flex-row justify-between items-center">
                    <span className="font-bold">{item.price} zł</span>
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
