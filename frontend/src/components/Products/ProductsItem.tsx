import { FC } from "react";
import { Link } from "react-router-dom";

interface ProductsItemProps {
  children: React.ReactNode;
  link: string;
}

const ProductsItem: FC<ProductsItemProps> = ({ link, children }) => {
  return (
    <Link to={link} className="rounded-lg w-full">
      <div>{children}</div>
    </Link>
  );
};

export default ProductsItem;
