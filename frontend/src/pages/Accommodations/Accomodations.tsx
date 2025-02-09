import { LatLngTuple } from "leaflet";
import { FC, useState } from "react";
import { CiLocationArrow1 } from "react-icons/ci";
import { useAccommodationList } from "src/api/AccommodationsAPI";
import { UseCategoryList } from "src/api/Category";
import { useDestinationList } from "src/api/Destinations";
import CreateDestinationModal from "src/components/Destinations/Modals/CreateDestinationModal";
import MapComponent from "src/components/Maps/MapComponen";
import ItemActions from "src/components/Products/Item/ItemActions";
import ItemActionsWrapper from "src/components/Products/Item/ItemActionsWrapper";
import ItemDescriptionWrapper from "src/components/Products/Item/ItemDescriptionWrapper";
import ItemFeatures from "src/components/Products/Item/ItemFeatures";
import ItemHeader from "src/components/Products/Item/ItemHeader";
import ItemImg from "src/components/Products/Item/ItemImg";
import ProductsItem from "src/components/Products/Item/ProductsItem";
import ProductsItemsSection from "src/components/Products/ProductsItemsSection";
import ProductsLayout from "src/components/Products/ProductsLayout";
import ProductsSectionHeader from "src/components/Products/ProductsSectionHeader";
import { Button } from "src/components/ui/button";
import useFilter from "src/hooks/UseFilter";
import CreateAccommodation from "./Create";
import CreateAccommodationForm from "src/components/Accommodation/Forms/CreateAccommodationForm";
import CreateAccomodationModal from "src/components/Accommodation/Modals/CreateDestinationModal";

interface AccommodationsProps {}

const Accommodations: FC<AccommodationsProps> = () => {
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

  const handleBoundsChange = (bounds: {
    northEastLat: number;
    northEastLng: number;
    southWestLat: number;
    southWestLng: number;
  }) => {
    updateFilter("northEastLat", bounds.northEastLat);
    updateFilter("northEastLng", bounds.northEastLng);
    updateFilter("southWestLat", bounds.southWestLat);
    updateFilter("southWestLng", bounds.southWestLng);
  };

  const { data: destination, isError } = useAccommodationList(filters);
  const {
    data: categories,
    isLoading,
    isError: CategoriesError,
  } = UseCategoryList();

  const validDestinations = destination.filter((d) => {
    if (!d.geoLocation?.latitude || !d.geoLocation?.longitude) {
      return false;
    }
    return true;
  });

  return (
    <ProductsLayout>
      <div className="flex flex-col gap-2 w-full xl:w-1/2">
        <ProductsSectionHeader title="Available places"></ProductsSectionHeader>
        <div className="flex flex-col h-auto xl:h-1 grow">
          <div className="flex flex-col gap-2 overflow-y-auto ">
            {destination.map((item, i) => (
              <ProductsItem link={`/destination/${item.id}`} key={i}>
                <ItemImg imgSrc={item.photoUrl} />
                <ItemDescriptionWrapper>
                  <ItemHeader title={item.name} />

                  <ItemActionsWrapper>
                    <ItemFeatures />
                    <ItemActions item={item} />
                  </ItemActionsWrapper>
                </ItemDescriptionWrapper>
              </ProductsItem>
            ))}
          </div>
        </div>
      </div>

      <ProductsItemsSection>
        <ProductsSectionHeader title="Destinations">
          <Button>Become partner</Button>
          <CreateAccomodationModal />
        </ProductsSectionHeader>

        <div className="maps-container mb-[2rem] h-[95%] flex flex-col grow">
          <MapComponent
            markers={validDestinations.map((d) => ({
              position: [
                d.geoLocation.latitude,
                d.geoLocation.longitude,
              ] as LatLngTuple,
              label: d.name,
              photoUrl: d.photoUrl,
              description: d.description,
            }))}
            onBoundsChange={handleBoundsChange}
          />
        </div>
      </ProductsItemsSection>
    </ProductsLayout>
  );
};

export default Accommodations;
