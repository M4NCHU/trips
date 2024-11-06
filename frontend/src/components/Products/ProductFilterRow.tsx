import { FC } from "react";

interface ProductFilterRowProps {
  title: string;
  children: React.ReactNode;
}

const ProductFilterRow: FC<ProductFilterRowProps> = ({ title, children }) => {
  return (
    <div className="flex flex-col gap-1 my-1">
      <div className="filter-row-title py-2">
        <h2>{title}</h2>
      </div>
      <div className="filter-row-children flex flex-row gap-2 justify-between">
        {children}
      </div>
    </div>
  );
};

export default ProductFilterRow;
