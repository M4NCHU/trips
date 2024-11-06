import { FC, useState } from "react";
import { BsCart } from "react-icons/bs";
import { MdOutlineAttachMoney } from "react-icons/md";
import { UseCategoryList } from "src/api/Category";
import { useDestinationList } from "src/api/Destinations";
import ProductFilterInput from "src/components/Products/ProductFilterInput";
import ProductFilterRow from "src/components/Products/ProductFilterRow";
import ProductsFilter from "src/components/Products/ProductsFilter";
import ProductsItem from "src/components/Products/ProductsItem";
import ProductsItemsList from "src/components/Products/ProductsItemsList";
import ProductsItemsSection from "src/components/Products/ProductsItemsSection";
import ProductsLayout from "src/components/Products/ProductsLayout";
import { ButtonWithIcon } from "src/components/ui/Buttons/ButtonWithIcon";
import { Button } from "src/components/ui/button";
import useFilter from "src/hooks/UseFilter";
import { IconName, isValidIconName } from "../../types/Icons/IconTypes";
import { CiLocationArrow1 } from "react-icons/ci";
import ProductsSectionHeader from "src/components/Products/ProductsSectionHeader";
import CreateDestinationModal from "src/components/Destinations/Modals/CreateDestinationModal";
import MapComponent from "src/components/Maps/MapComponen";

interface DestinationsProps {}

const Destinations: FC<DestinationsProps> = () => {
  const { filters, updateFilter, resetFilters } = useFilter();

  const markers: { position: [number, number]; label: string }[] = [
    { position: [49.555261997701884, 21.624581318696112], label: "Londyn" },
    { position: [51.51, -0.1], label: "Drugi punkt w Londynie" },
  ];
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
      <ProductsFilter applyFilter={applyFilters}>
        <ProductFilterRow title={"Price"}>
          <ProductFilterInput
            icon={<MdOutlineAttachMoney />}
            label="Min price"
            type="number"
            value={minPrice ?? ""}
            onChange={(e) => setMinPrice(Number(e.target.value) || null)}
          />
          <ProductFilterInput
            icon={<MdOutlineAttachMoney />}
            label="Max price"
            type="number"
            value={maxPrice ?? ""}
            onChange={(e) => setMaxPrice(Number(e.target.value) || null)}
          />
        </ProductFilterRow>
        <ProductFilterRow title={"Choose categories"}>
          <div className="flex flex-row gap-2 flex-wrap">
            <Button onClick={() => setSelectedCategory(null)}>All</Button>
            {categories &&
              categories.map((item) => (
                <Button
                  icon={isValidIconName(item.icon) ? item.icon : undefined}
                  key={item.id}
                  variant={"defaultSecondary"}
                  onClick={() => setSelectedCategory(item.id)}
                  className={
                    selectedCategory === item.id ? "border-blue-500" : ""
                  }
                >
                  {item.name}
                </Button>
              ))}
          </div>
        </ProductFilterRow>
      </ProductsFilter>

      <ProductsItemsSection>
        <ProductsSectionHeader title="Destinations">
          <Button>Become partner</Button>
          <CreateDestinationModal />
        </ProductsSectionHeader>

        <div className="maps-container mb-[2rem]">
          <MapComponent markers={markers} />
        </div>
        <ProductsItemsList>
          {destination.map((item, i) => (
            <ProductsItem link={`/destination/${item.id}`} key={i}>
              <div className="flex flex-col gap-2">
                <div className="w-full h-48 overflow-hidden">
                  <img
                    src={item.photoUrl}
                    alt="destination"
                    className="w-full h-full object-cover rounded-lg"
                  />
                </div>
                <div className="flex flex-col gap-2 p-2">
                  <h4 className="text-lg font-semibold">{item.name}</h4>
                  <p className="text-foreground flex flex-row items-center gap-2 text-sm">
                    <CiLocationArrow1 />
                    {item.description}
                  </p>
                  <div className="flex flex-row justify-between items-center">
                    <div className="product-price flex flex-row items-center gap-2 ">
                      <h3 className="font-bold text-2xl">{item.price} zł</h3>
                      <span className="text-foreground text-base">/night</span>
                    </div>
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
