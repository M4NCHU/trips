import { FC, useState } from "react";
import { UseCategoryList } from "src/api/Category";
import { useDestinationList } from "src/api/Destinations";
import useFilter from "src/hooks/UseFilter";
import { Destination } from "src/types/Destination";
import { Button } from "src/components/ui/button";
import Table from "src/components/ui/Table/Table";
import FilterModal from "src/components/Modals/FilterModal";
import CreateDestinationModal from "src/components/Destinations/Modals/CreateDestinationModal";

interface DestinationsProps {}

const DestinationsAdmin: FC<DestinationsProps> = () => {
  const { filters, updateFilter, resetFilters } = useFilter();
  const columns: (keyof Destination)[] = ["name", "price", "visitPlaces"];

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

  const { data: destinations, isError } = useDestinationList(filters);
  const {
    data: categories,
    isLoading,
    isError: CategoriesError,
  } = UseCategoryList();

  const filtersList = [
    {
      name: "Category",
      type: "select",
      value: selectedCategory ?? "",
      setValue: setSelectedCategory,
      options: categories ? categories.map((category) => category.name) : [],
    },
    {
      name: "Min Price",
      type: "number",
      value: minPrice ?? "",
      setValue: setMinPrice,
      minValue: 0,
      maxValue: maxPrice ?? undefined,
    },
    {
      name: "Max Price",
      type: "number",
      value: maxPrice ?? "",
      setValue: setMaxPrice,
      minValue: minPrice ?? undefined,
    },
    {
      name: "Min Rating",
      type: "number",
      value: minRating ?? "",
      setValue: setMinRating,
      minValue: 0,
      maxValue: maxRating ?? undefined,
    },
    {
      name: "Max Rating",
      type: "number",
      value: maxRating ?? "",
      setValue: setMaxRating,
      minValue: minRating ?? undefined,
    },
  ];

  if (isError) return <div>Error loading destinations...</div>;

  return (
    <div className="p-4">
      <h1 className="text-xl font-bold mb-4">Manage Destinations</h1>

      <div className="flex flex-wrap gap-4 mb-4">
        <FilterModal filters={filtersList} applyFilters={applyFilters} />

        <Button
          onClick={applyFilters}
          className="bg-blue-500 text-white rounded px-4 py-2"
        >
          Apply Filters
        </Button>
        <Button
          onClick={resetFilters}
          className="bg-gray-500 text-white rounded px-4 py-2"
        >
          Reset Filters
        </Button>
      </div>

      <div className="p-0 md:p-4">
        <h1 className="text-xl font-bold mb-4">Destinations List</h1>

        <Table
          data={destinations}
          createModal={<CreateDestinationModal />}
          tableTitle="Destinations list"
          tableDescription="Destinations list description"
          columns={columns}
          renderCell={(item, column) => {
            const cellData = item[column];

            if (Array.isArray(cellData)) {
              return (
                <ul>
                  {cellData.map((place, index) => (
                    <li key={index}>{place.name}</li>
                  ))}
                </ul>
              );
            }

            if (column === "price") {
              return `${cellData} zł`;
            }

            return cellData as React.ReactNode;
          }}
        />
      </div>
    </div>
  );
};

export default DestinationsAdmin;
