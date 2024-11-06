import { FC } from "react";

interface ProductsItemsListProps {
  children: React.ReactNode;
}

const ProductsItemsList: FC<ProductsItemsListProps> = ({ children }) => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 w-full">
      {children}
    </div>
  );
};

export default ProductsItemsList;
