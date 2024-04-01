import { FC } from "react";

interface ProductsItemsListProps {
  children: React.ReactNode;
}

const ProductsItemsList: FC<ProductsItemsListProps> = ({ children }) => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols:5 gap-4">
      {children}
    </div>
  );
};

export default ProductsItemsList;
