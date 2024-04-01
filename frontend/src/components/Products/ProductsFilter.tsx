import { FC } from "react";

interface ProductsFilterProps {
  children: React.ReactNode;
}

const ProductsFilter: FC<ProductsFilterProps> = ({ children }) => {
  return (
    <div className="w-1/3 hidden md:block">
      <div className="sticky top-5 bg-secondary rounded-lg flex flex-col p-4">
        <div className="flex flex-row justify-between py-2 border-b-[1px] border-background">
          <div className="flex items-center">
            <h1 className="text-xl font-semibold">Filter</h1>
          </div>
        </div>
        <div className="flex flex-col py-2 gap-2">{children}</div>
      </div>
    </div>
  );
};

export default ProductsFilter;
