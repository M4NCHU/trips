import { FC } from "react";

interface ProductsItemsSectionProps {
  children: React.ReactNode;
}

const ProductsItemsSection: FC<ProductsItemsSectionProps> = ({ children }) => {
  return <div className="grow bg-secondary flex-col p-4">{children}</div>;
};

export default ProductsItemsSection;
