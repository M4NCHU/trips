import { FC } from "react";
import { Button } from "../ui/button";
import { MdOutlineFilterList } from "react-icons/md";

interface ProductsFilterProps {
  applyFilter: () => void;
  children: React.ReactNode;
}

const ProductsFilter: FC<ProductsFilterProps> = ({ applyFilter, children }) => {
  return (
    <div className="w-2/5 min-w-[24rem] md:block flex flex-col">
      <div className="bg-secondary rounded-lg flex flex-col p-4 grow h-full">
        <div className="flex flex-row justify-between py-2 border-b-[1px] border-background">
          <div className="flex items-center justify-between w-full">
            <h1 className="text-2xl font-semibold">Filters</h1>
            <Button className="" variant={"ghost"}>
              <MdOutlineFilterList className="text-2xl font-semibold" />
            </Button>
          </div>
        </div>
        <div className="flex flex-col py-2 gap-2 grow overflow-auto">
          {children}
        </div>
        <Button variant={"default"} onClick={applyFilter}>
          Filtruj
        </Button>
      </div>
    </div>
  );
};

export default ProductsFilter;
