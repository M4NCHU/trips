import { FC, useState } from "react";
import { IoCreate } from "react-icons/io5";
import { Button } from "../ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../ui/dialog";

interface FilterModalProps {
  filters: {
    name: string;
    type: string;
    value: any;
    setValue: (value: any) => void;
    options?: string[];
    regex?: RegExp;
    validationMessage?: string;
    minValue?: number;
    maxValue?: number;
  }[];
  applyFilters: () => void;
}

const FilterModal: FC<FilterModalProps> = ({ filters, applyFilters }) => {
  const [errors, setErrors] = useState<string[]>([]);

  const validateFilters = () => {
    const newErrors: string[] = [];

    filters.forEach((filter) => {
      // Sprawdź wyrażenie regularne (jeśli dotyczy)
      if (filter.regex && !filter.regex.test(filter.value)) {
        newErrors.push(
          filter.validationMessage || `Invalid value for ${filter.name}`
        );
      }

      if (filter.minValue !== undefined && filter.value < filter.minValue) {
        newErrors.push(`${filter.name} cannot be less than ${filter.minValue}`);
      }

      if (filter.maxValue !== undefined && filter.value > filter.maxValue) {
        newErrors.push(`${filter.name} cannot be more than ${filter.maxValue}`);
      }

      if (filter.name === "Min Price") {
        const maxPriceFilter = filters.find((f) => f.name === "Max Price");

        if (
          maxPriceFilter &&
          maxPriceFilter.value !== "" &&
          maxPriceFilter.value !== null &&
          filter.value > maxPriceFilter.value
        ) {
          newErrors.push("Min price cannot be greater than max price.");
        }
      }

      if (filter.name === "Min Rating") {
        const maxRatingFilter = filters.find((f) => f.name === "Max Rating");

        if (
          maxRatingFilter &&
          maxRatingFilter.value !== "" &&
          maxRatingFilter.value !== null &&
          filter.value > maxRatingFilter.value
        ) {
          newErrors.push("Min rating cannot be greater than max rating.");
        }
      }
    });

    setErrors(newErrors);
    return newErrors.length === 0;
  };

  const handleApplyFilters = () => {
    if (validateFilters()) {
      applyFilters();
    }
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button className="bg-purple-500 text-foreground">
          <IoCreate />
          <span className="hidden md:block">Filter</span>
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Filter Destinations</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col gap-4">
          {filters.map((filter, index) => (
            <div key={index} className="flex flex-col">
              <label className="font-semibold">{filter.name}</label>
              {filter.type === "select" ? (
                <select
                  value={filter.value}
                  onChange={(e) => filter.setValue(e.target.value)}
                  className="border p-2 rounded"
                >
                  <option value="">All {filter.name}</option>
                  {filter.options?.map((option, i) => (
                    <option key={i} value={option}>
                      {option}
                    </option>
                  ))}
                </select>
              ) : (
                <input
                  type={filter.type}
                  value={filter.value}
                  onChange={(e) => filter.setValue(e.target.value)}
                  className="border p-2 rounded"
                  placeholder={`Enter ${filter.name}`}
                />
              )}
            </div>
          ))}

          {errors.length > 0 && (
            <div className="text-red-500">
              <ul>
                {errors.map((error, i) => (
                  <li key={i}>{error}</li>
                ))}
              </ul>
            </div>
          )}
        </div>

        <DialogFooter>
          <Button
            onClick={handleApplyFilters}
            className="bg-blue-500 text-white"
          >
            Apply
          </Button>
          <DialogClose asChild>
            <Button type="button" variant="secondary">
              Close
            </Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default FilterModal;
