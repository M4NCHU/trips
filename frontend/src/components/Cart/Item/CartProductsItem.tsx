import { FC } from "react";
import { Link } from "react-router-dom";

interface ProductsItemProps {
  children: React.ReactNode;
  link: string;
}

const CartProductsItem: FC<ProductsItemProps> = ({ link, children }) => {
  return (
    <Link
      to={link}
      className="rounded-lg w-full flex flex-row gap-[1rem] bg-background p-4"
    >
      {children}
    </Link>
  );
};

export default CartProductsItem;
