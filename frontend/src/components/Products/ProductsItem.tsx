import { FC } from "react";
import { Link } from "react-router-dom";

interface ProductsItemProps {
  children: React.ReactNode;
  link: string;
}

const ProductsItem: FC<ProductsItemProps> = ({ link, children }) => {
  return (
    <Link to={link}>
      <div className="bg-background p-2 rounded-lg">{children}</div>
    </Link>
  );
};

export default ProductsItem;
