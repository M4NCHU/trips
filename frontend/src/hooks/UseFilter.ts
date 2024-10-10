import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { debounce } from "lodash"; // Możesz użyć lodash lub własnej implementacji debounce

type FilterValues = string | number | undefined;

interface Filters {
  [key: string]: FilterValues;
}

function useFilter(initialFilters: Filters = {}, delay: number = 500) {
  const [filters, setFilters] = useState<Filters>(initialFilters);
  const navigate = useNavigate();
  const location = useLocation();

  // Używamy debounce, aby odroczyć aktualizację URL do momentu zakończenia wprowadzania filtrów
  const updateURLWithFilters = debounce((newFilters: Filters) => {
    const searchParams = new URLSearchParams();

    Object.keys(newFilters).forEach((key) => {
      const value = newFilters[key];
      if (value !== undefined && value !== null && value !== "") {
        searchParams.set(key, String(value));
      }
    });

    navigate({
      pathname: location.pathname,
      search: searchParams.toString(),
    });
  }, delay);

  useEffect(() => {
    updateURLWithFilters(filters);
    return () => updateURLWithFilters.cancel();
  }, [filters]);

  const updateFilter = (key: string, value: FilterValues) => {
    setFilters((prevFilters) => ({
      ...prevFilters,
      [key]: value,
    }));
  };

  const resetFilters = () => {
    setFilters(initialFilters);
  };

  return { filters, updateFilter, resetFilters };
}

export default useFilter;
