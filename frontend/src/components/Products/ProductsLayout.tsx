import { FC } from "react";

interface ProductsLayoutProps {
  children: React.ReactNode;
}

const ProductsLayout: FC<ProductsLayoutProps> = ({ children }) => {
  return <div className="flex flex-row gap-2 grow py-2 px-2">{children}</div>;
};

export default ProductsLayout;
